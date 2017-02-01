using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HoeGraphForm
{
    public partial class Form1 : Form
    {
        //static string date = "\\201701181613";
        static string date = "\\201701181627";
        static string path;
        static int JointNum = 7; //左手
        static double max = 0; //速さの最大値

        private List<double[,]> kinectPoint = new List<double[,]>();
        private List<double[,]> kinectPointNorm = new List<double[,]>();
        private List<double[]> kinectVec = new List<double[]>();
        private List<double[]> kinectVecSmooth = new List<double[]>();
        private List<double[]> wii = new List<double[]>();
        private List<int>[] frameKinectToWii = new List<int>[2];
        private List<int>[] frameWiiToKinect = new List<int>[2];
        private List<int> bmpNum = new List<int>();
        private List<int>[] minimum = new List<int>[21];

        private VectorBuilder<double> V = Vector<double>.Build;
        private MatrixBuilder<double> M = Matrix<double>.Build;

        public Form1()
        {
            InitializeComponent();
            FileToList();
            SmoothingVec();
            SearchMin();
            PrepareBmpFile();
            ListToGraph();
            WriteVec();
        }

        //ファイルからリストを作成
        private void FileToList()
        { 
            path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\HoeData" + date;
            string line;
            System.IO.StreamReader file =
                new System.IO.StreamReader(path+"\\Kinect.csv");

            int lineCount = 0;
            double[,] point = new double[21, 3];
            frameKinectToWii[0] = new List<int>();
            frameKinectToWii[1] = new List<int>();
            frameWiiToKinect[0] = new List<int>();
            frameWiiToKinect[1] = new List<int>();
            while ((line = file.ReadLine()) != null)
            {
                string[] token = line.Split(',');
                int index = lineCount++ % 21;
                if (index == 0) {
                    frameKinectToWii[0].Add(int.Parse(token[0]));//frameListを追加
                    frameKinectToWii[1].Add(int.Parse(token[1]));//frameListを追加
                }
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
                frameWiiToKinect[0].Add(int.Parse(token[0]));//frameListを追加
                frameWiiToKinect[1].Add(int.Parse(token[1]));//frameListを追加
                for (int i = 0; i < 3; i++) accel[i] = double.Parse(token[i + 2]);
                //Console.WriteLine(accel[0]+","+accel[1]+","+accel[2]);
                wii.Add((double[])accel.Clone());
                lineCount++;
            }
            file.Close();
        }

        //座標から速さを求める
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

        //座標を正規化
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

        //スムージングをした速さを求める
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

        //極小値を探す
        public void SearchMin()
        {
            for (int i=0; i<21; i++)
            {
                minimum[i] = new List<int>();
            }
            max = kinectVecSmooth[0][JointNum];
            for (int i = 1; i < kinectVecSmooth.Count - 1; i++)
            {
                if (kinectVecSmooth[i][JointNum] > max) max = kinectVecSmooth[i][JointNum];
                for (int j = 0; j < 21; j++)
                {
                    if (kinectVecSmooth[i - 1][j] > kinectVecSmooth[i][j] &&
                       kinectVecSmooth[i][j] > kinectVecSmooth[i + 1][j])
                        minimum[j].Add(i);
                }
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

        //リストをグラフに
        private void ListToGraph()
        {
            kinectChart.Series[0].Name = "Light_HAND";
            kinectChart.Series[0].ChartType = SeriesChartType.Line;
            kinectChart.Series[0].BorderWidth = 1;
            for (int i = 0; i < kinectVecSmooth.Count; i++)
                kinectChart.Series[0].Points.AddXY(frameKinectToWii[0][i+1], kinectVecSmooth[i][JointNum]);
                //kinectChart.Series[0].Points.AddXY(frameKinectToWii[0][i+1], kinectVec[i][JointNum]);

            wiiChart.Series[0].Name = "WiiAccel";
            wiiChart.Series[0].ChartType = SeriesChartType.Line;
            wiiChart.Series[0].BorderWidth = 1;
            for (int i = 0; i < wii.Count; i++)
            {
                double Y = Math.Sqrt(wii[i][0]* wii[i][0]+ wii[i][1]* wii[i][1]+ wii[i][2]* wii[i][2]);
                wiiChart.Series[0].Points.AddXY(frameWiiToKinect[0][i], Y);
            }

            kinectChart.Series.Add(JointNum.ToString());
            kinectChart.Series.Add("bmp");

            kinectChart.Series[1].ChartType = SeriesChartType.BoxPlot;
            kinectChart.Series[1].BorderWidth = 1;
            kinectChart.Series[2].ChartType = SeriesChartType.BoxPlot;
            kinectChart.Series[2].BorderWidth = 1;
        }

        //rgbBarの値が変わった時のイベント
        private void rgbBar_ValueChanged(object sender, EventArgs e)
        {
            int bmpindex = FindBmpIndex(rgbBar.Value);
            int vecindex = FindVec(rgbBar.Value);
            rgbImage.ImageLocation = @path + "\\bmp\\" + bmpindex + ".bmp";
            barLabel.Text = rgbBar.Value.ToString();
            jointSpeed.Text = kinectVecSmooth[vecindex][JointNum].ToString();

            kinectChart.Series[2].Points.Clear();
            kinectChart.Series[2].Points.AddXY(frameKinectToWii[0][vecindex+1],kinectVecSmooth[vecindex][JointNum].ToString());
        }

        //bmpファイルの番号があるかどうかを調べる
        private int FindBmpIndex(int index)
        {
            if (bmpNum.Contains(index)) return index;
            if (index > 0) return FindBmpIndex(index - 1);
            return -1;
        }

        //時刻tの速さのデータがあるか調べる
        private int FindVec(int t)
        {
            if (t < frameKinectToWii[0][0]) return 0;
            if (frameKinectToWii[0].Contains(t)) return frameKinectToWii[0].IndexOf(t);
            else return FindVec(t - 1);
        }

        //thresholdBarが動いたときのイベント
        private void thresholdBar_ValueChanged(object sender, EventArgs e)
        {
            kinectChart.Series[1].Points.Clear();
            double T = thresholdBar.Value * 0.001;
            thresholdLabel.Text = T.ToString();
            kinectChart.Series[1].ChartType = SeriesChartType.BoxPlot;
            kinectChart.Series[1].BorderWidth = 1;
            foreach (var min in minimum[JointNum])
            {
                if (kinectVecSmooth[min][JointNum] < T)
                    kinectChart.Series[1].Points.AddXY(frameKinectToWii[0][min], max);
            }
        }

        private void WriteVec()
        {
            using (StreamWriter SW = new StreamWriter(path + "\\Vec" + JointNum + ".csv", false))
            {
                SW.WriteLine();
                Console.WriteLine(frameKinectToWii[0].Count + "," + kinectVecSmooth.Count);
                for (int i = 1; i < frameKinectToWii[0].Count; i++)
                {
                    SW.WriteLine(frameKinectToWii[0][i] + ","
                                + kinectVecSmooth[i-1][JointNum]);
                }
            }
        }
    }
}