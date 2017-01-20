using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HoeGraphForm
{
    public partial class Form1 : Form
    {
        static string date = "\\201701181627";
        static string path;

        private List<double[,]> kinectPoint = new List<double[,]>();
        private List<double[,]> kinectPointNorm = new List<double[,]>();
        private List<double[]> kinectVec = new List<double[]>();
        private List<double[]> kinectVecSmooth = new List<double[]>();
        private List<double[]> wii = new List<double[]>();
        private List<int[]> frameKinectToWii = new List<int[]>();
        private List<int[]> frameWiiToKinect = new List<int[]>();
        private List<int> bmpNum = new List<int>();

        private VectorBuilder<double> V = Vector<double>.Build;
        private MatrixBuilder<double> M = Matrix<double>.Build;

        public Form1()
        {
            InitializeComponent();
            FileToList();
            SmoothingVec();
            PrepareBmpFile();
            ListToGraph();
        }

        private void FileToList()
        { 
            path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\HoeData" + date;
            string line;
            System.IO.StreamReader file =
                new System.IO.StreamReader(path+"\\Kinect.csv");

            int lineCount = 0;
            double[,] point = new double[21, 3];
            while ((line = file.ReadLine()) != null)
            {
                string[] token = line.Split(',');
                int index = lineCount++ % 21;
                if (index == 0)
                    frameKinectToWii.Add(new int[2] { int.Parse(token[0]), int.Parse(token[1])} );//frameListを追加
                for (int i = 0; i < 3; i++)
                {
                    point[index, i] = double.Parse(token[i + 2]);
                    //Console.WriteLine(double.Parse(token[i + 2]));
                }
                if(index == 20)
                {
                    kinectPoint.Add((double[,])point.Clone());
                    //foreach (var a in kinectPoint[kinectPoint.Count - 1]) Console.WriteLine(a);
                    PointToVec(kinectPoint.Count-1);//座標から速さを計算
                    PointToNorm(kinectPoint.Count - 1);//座標を正規化
                }
            }
            file.Close();

            lineCount = 0;
            double[] accel = new double[3];
            file = new System.IO.StreamReader(path + "\\Wii.csv");
            while ((line = file.ReadLine()) != null)
            {
                string[] token = line.Split(',');
                //Console.WriteLine(line);
                frameWiiToKinect.Add(new int[2] { int.Parse(token[0]), int.Parse(token[1]) });//frameListを追加
                for (int i = 0; i < 3; i++) accel[i] = double.Parse(token[i + 2]);
                //Console.WriteLine(accel[0]+","+accel[1]+","+accel[2]);
                wii.Add((double[])accel.Clone());
                lineCount++;
            }
            file.Close();
        }

        private void PointToVec(int tail)
        {
            if (tail == 0) return;
            double[] vec = new double[21];
            for (int i = 0; i < 21; i++)
            {
                double dx = kinectPoint[tail][i, 0] - kinectPoint[tail - 1][i, 0];
                double dy = kinectPoint[tail][i, 1] - kinectPoint[tail - 1][i, 1];
                double dz = kinectPoint[tail][i, 2] - kinectPoint[tail - 1][i, 2];
                vec[i] = Math.Sqrt(dx*dx+dy*dy+dz*dz);
            }
            kinectVec.Add((double[])vec.Clone());
        }

        private void PointToNorm(int tail)
        {
            //体が常に横を向くように正規化
            //HipRightからHipLeftへのびるベクトルに対して垂直なベクトルを計算
            double[] hip = new double[3] { -kinectPoint[tail][12, 2] - kinectPoint[tail][16, 2],
                                           0,
                                           kinectPoint[tail][12, 0] - kinectPoint[tail][16, 0]};
            var VecHip = V.DenseOfArray(hip);
            //求めたベクトルの絶対値を1に
            var e_x = VecHip.Divide((float)VecHip.L2Norm());
            //e_y = (0,1,0)
            var e_y = V.DenseOfArray(new double[3] { 0, 1, 0 });
            //e_z = e_x × e_y
            var e_z = V.DenseOfArray(new double[3] { -VecHip.At(2), 0, VecHip.At(0) });//1
            //求めたベクトルの絶対値を1に
            e_z = e_z.Divide((float)e_z.L2Norm());

            //e_x,e_y,e_zを一つの行列に
            var mat = M.DenseOfColumnVectors(new Vector<double>[] { e_x, e_y, e_z }).Inverse();
            double[,] norm = new double[21,3];
            for (int i = 0; i < 21; i++)
            {
                var VecJ = V.DenseOfArray(new double[3] { kinectPoint[tail][i, 0]-kinectPoint[tail][0,0],
                                                             kinectPoint[tail][i, 1]-kinectPoint[tail][0,1],
                                                             kinectPoint[tail][i, 2]-kinectPoint[tail][0,2] });
                var E = mat * VecJ;
                for(int j=0;j<3;j++)
                norm[i,j]=E.At(j);
            }
            kinectPointNorm.Add((double[,])norm.Clone());
        }

        private void SmoothingVec()
        {
            double SY = 0;
            double[] Y = new double[21];
            for (int i=0; i<kinectVec.Count; i++)
            {
                for(int j = 0; j < 21; j++)
                {
                    if (i < 3)
                    {
                        for (int k = 0; k < i+2; k++) SY += kinectVec[k][j];
                        Y[j] = SY / (3+i);
                        SY = 0;
                    }
                    else if ( i > kinectVec.Count - 3)
                    {
                        for (int k = kinectVec.Count-1; k < i - 2; k--) SY += kinectVec[k][j];
                        Y[j] = SY / (3 + kinectVec.Count-1-i);
                        SY = 0;
                    }
                    else
                    {
                        for (int k = 0; k < 5; k++) SY += kinectVec[i + k - 2][j];
                        //Console.WriteLine(SY);
                        Y[j] = SY / 5;
                        SY = 0;
                    }
                }
                kinectVecSmooth.Add((double[])Y.Clone());
            }
        }

        /// <summary>
        /// パスからbmpファイルの名前を取得しリストにする
        /// </summary>
        private void PrepareBmpFile()
        {
            string[] files = System.IO.Directory.GetFiles(path+"\\bmp", "*");
            foreach (var file in files)
            {
                //bmpファイルのパスからbmp番号をtoken2[0]に格納
                string[] token1 = file.Split('\\');
                string[] token2 = token1[7].Split('.');
                //bmpの番号をリストに追加
                bmpNum.Add(int.Parse(token2[0]));
            }
            //bmpの番号順にソート
            bmpNum.Sort();
            rgbBar.Minimum = bmpNum[0];
            rgbBar.Maximum = bmpNum[bmpNum.Count-1];
            barLabel.Text = bmpNum[0].ToString();

            rgbImage.Image = null;
            int bmpindex = FindBmpIndex(bmpNum[0]);
            rgbImage.ImageLocation = @path + "\\bmp\\" + bmpindex + ".bmp";
        }

        private void ListToGraph()
        {
            kinectChart.Series[0].Name = "Light_HAND";
            kinectChart.Series[0].ChartType = SeriesChartType.Line;
            kinectChart.Series[0].BorderWidth = 1;
            for (int i = 0; i < kinectVecSmooth.Count; i++)
                kinectChart.Series[0].Points.AddXY(frameKinectToWii[i+1][0], kinectVecSmooth[i][7]);

            wiiChart.Series[0].Name = "WiiAccel";
            wiiChart.Series[0].ChartType = SeriesChartType.Line;
            wiiChart.Series[0].BorderWidth = 1;
            for (int i = 0; i < wii.Count; i++)
            {
                double Y = Math.Sqrt(wii[i][0]* wii[i][0]+ wii[i][1]* wii[i][1]+ wii[i][2]* wii[i][2]);
                wiiChart.Series[0].Points.AddXY(frameWiiToKinect[i][0], Y);
            }
        }

        private void rgbBar_ValueChanged(object sender, EventArgs e)
        {
            int bmpindex = FindBmpIndex(rgbBar.Value);
            rgbImage.ImageLocation = @path + "\\bmp\\" + bmpindex + ".bmp";
            barLabel.Text = bmpindex.ToString();
            //jointSpeed.Text = kinectVecSmooth[rgbBar.Value-rgbBar.Minimum][7].ToString();
            jointSpeed.Text = kinectVecSmooth.Count.ToString();
            //jointSpeed.Text = rgbBar.Minimum.ToString();
            wiiAccel.Text = "?";
        }

        private int FindBmpIndex(int index)
        {
            if (bmpNum.Contains(index)) return index;
            if (index > 0) return FindBmpIndex(index - 1);
            return -1;
        }
    }
}
