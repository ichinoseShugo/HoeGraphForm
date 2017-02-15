using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HoeGraphForm
{
    public partial class Form1 : Form
    {
        //public static string date = "201701181613";
        public static string date = "201701181627";
        string[] Joint = new string[21];
        static int JointNum = 7; //左手
        static int kinectT;
        static int wiiT;
        File file;
        List list;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            InitializeComponent();
            file = new File(date);
            list = new List(file);
            InitString();
            InitValue();
            ListToGraph();
        }
        /// <summary>
        /// 関節番号
        /// </summary>
        private void InitString()
        {
            Joint[0] = "HipCenter";
            Joint[1] = "Spine";
            Joint[2] = "ShoulderCenter";
            Joint[3] = "Head";
            Joint[4] = "ShoulderLeft";
            Joint[5] = "ElbowLeft";
            Joint[6] = "WristLeft";
            Joint[7] = "HandLeft";
            Joint[8] = "ShoulderRight";
            Joint[9] = "ElbowRight";
            Joint[10] = "WristRight";
            Joint[11] = "HandRight";
            Joint[12] = "HipLeft";
            Joint[13] = "KneeLeft";
            Joint[14] = "AnkleLeft";
            Joint[15] = "FootLeft";
            Joint[16] = "HipRight";
            Joint[17] = "KneeRight";
            Joint[18] = "AnkleRight";
            Joint[19] = "FootRight";
            Joint[20] = "Centroid";
        }
        /// <summary>
        /// Formの各値を初期化
        /// </summary>
        private void InitValue()
        {
            kinectT = Caliculate.Frequency(list.kinectSmoothedVelocity[JointNum]);
            kinectFreq.Text = "T:" + kinectT.ToString();
            wiiT = Caliculate.Frequency(list.wii);
            wiiFreq.Text = "T:" + wiiT;

            rgbBar.Maximum = list.NumOfBmp[list.NumOfBmp.Count-1];
            rgbBar.Minimum = list.NumOfBmp[0];
            barLabel.Text = rgbBar.Value.ToString();

            thresholdLabel.Text = "threshold:0";
            rgbImage.ImageLocation = file.bmpPath + list.NumOfBmp[0] + ".bmp";
        }
        /// <summary>
        /// リストからグラフを生成
        /// </summary>
        private void ListToGraph()
        {
            kinectChart.Series[0].Name = Joint[JointNum];
            kinectChart.Series[0].ChartType = SeriesChartType.Line;
            kinectChart.Series[0].BorderWidth = 1;
            kinectChart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            for (int i = 0; i < list.kinectSmoothedVelocity[JointNum].Count; i++)
                kinectChart.Series[0].Points.AddXY(list.frameKinectToWii[0][i], list.kinectSmoothedVelocity[JointNum][i]);

            AddSeries("primitive",kinectChart);
            AddSeries("bmp",kinectChart);
            AddSeries("T", kinectChart);

            wiiChart.Series[0].Name = "WiiAccel";
            wiiChart.Series[0].ChartType = SeriesChartType.Line;
            wiiChart.Series[0].BorderWidth = 1;
            for (int i = 0; i < list.wii.Count; i++)
                wiiChart.Series[0].Points.AddXY(list.frameWiiToKinect[0][i], list.wii[i][3]);

            AddSeries("primitive",wiiChart);
            AddSeries("bmp", wiiChart);
            AddSeries("T",wiiChart);

            kinectChart.Series[3].Color = System.Drawing.Color.DarkGreen;
            wiiChart.Series[3].Color = System.Drawing.Color.DarkGreen;
        }
        /// <summary>
        /// グラフを追加
        /// </summary>
        private void AddSeries(string name,Chart chart)
        {
            chart.Series.Add(name);
            chart.Series[chart.Series.Count - 1].ChartType = SeriesChartType.BoxPlot;
            chart.Series[chart.Series.Count - 1].BorderWidth = 1;
        }
        /// <summary>
        /// bmpファイルがあるか調べる
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int FindBmpIndex(int index)
        {
            if (list.NumOfBmp.Contains(index)) return index;
            if (index > 0) return FindBmpIndex(index - 1);
            return -1;
        }
        /// <summary>
        /// 基本周波数を元にグラフを動作プリミティブに分割
        /// </summary>
        private void DivideGraph(int vecindex)
        {
            int bmpframe = list.frameKinectToWii[0][vecindex];
            int tailframe = list.frameKinectToWii[0][list.frameKinectToWii[0].Count - 1];
            int tailindex = list.frameKinectToWii[0].Count - 1;
            int vecframe = list.frameKinectToWii[0][vecindex + kinectT];

            kinectChart.Series[3].Points.Clear();

            for (int i = 1; i * kinectT + vecindex < tailframe; i++)
            {
                double min = list.max[JointNum];
                int minindex = 0;
                for (int j = -15; j < 15; j++)
                {
                    int index = j + i * kinectT + vecindex;
                    if (0 > index || tailindex < index) continue;
                    if (list.kinectSmoothedVelocity[JointNum][index - 1] > list.kinectSmoothedVelocity[JointNum][index] &&
                       list.kinectSmoothedVelocity[JointNum][index] < list.kinectSmoothedVelocity[JointNum][index + 1] &&
                       list.kinectSmoothedVelocity[JointNum][index] < min)
                    {
                        min = list.kinectSmoothedVelocity[JointNum][index];
                        minindex = index;
                    }
                }
                kinectChart.Series[3].Points.AddXY(list.frameKinectToWii[0][minindex], list.max[JointNum]);
            }
                //kinectChart.Series[3].Points.AddXY(i*kinectT+list.frameKinectToWii[0][vecindex], list.max[JointNum]);
        
            }
        /// <summary>
        /// 時刻tの速さのデータがあるか調べる
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private int FindVec(int t)
        {
            if (t < list.frameKinectToWii[0][0]) return 0;
            if (list.frameKinectToWii[0].Contains(t)) return list.frameKinectToWii[0].IndexOf(t);
            else return FindVec(t - 1);
        }
        /// <summary>
        /// 速さのデータの地点がWiiのデータのどの地点にあるか調べる
        /// </summary>
        /// <param name="vecindex"></param>
        /// <returns>wiiのインデックス</returns>
        private int FindWiiIndex(int vecindex)
        {
            return list.frameWiiToKinect[0].IndexOf(list.frameKinectToWii[1][vecindex]);
        }
        /// <summary>
        /// rgbBarが変化した時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rgbBar_ValueChanged(object sender, EventArgs e)
        {
            int bmpindex = FindBmpIndex(rgbBar.Value);
            int vecindex = FindVec(rgbBar.Value);
            int wiiindex = FindWiiIndex(vecindex);

            rgbImage.ImageLocation = file.bmpPath + bmpindex + ".bmp";
            barLabel.Text = rgbBar.Value.ToString();
            jointSpeed.Text = list.kinectSmoothedVelocity[JointNum][vecindex].ToString();
            wiiFrame.Text = list.frameKinectToWii[1][vecindex].ToString();
            wiiAccel.Text = list.wii[FindWiiIndex(vecindex)][3].ToString();

            kinectChart.Series[2].Points.Clear();
            kinectChart.Series[2].Points.AddXY(list.frameKinectToWii[0][vecindex], list.kinectSmoothedVelocity[JointNum][vecindex]);

            DivideGraph(vecindex);

            wiiChart.Series[2].Points.Clear();
            wiiChart.Series[2].Points.AddXY(list.frameKinectToWii[1][vecindex], list.wii[wiiindex][3]);
            /*
            wiiChart.Series[3].Points.Clear();
            for (int i = 1; i * wiiT + list.frameKinectToWii[1][vecindex] < list.frameWiiToKinect[0][list.frameWiiToKinect[0].Count - 1]; i++)
                wiiChart.Series[3].Points.AddXY(i * wiiT + list.frameKinectToWii[1][vecindex], 5);
            */    
    }
        /// <summary>
        /// thresholdBarが動いた時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void thresholdBar_ValueChanged(object sender, EventArgs e)
        {
            kinectChart.Series[1].Points.Clear();
            double T = thresholdBar.Value * 0.001;
            thresholdLabel.Text = "threshold:"+T.ToString();
            kinectChart.Series[1].ChartType = SeriesChartType.BoxPlot;
            kinectChart.Series[1].BorderWidth = 1;
            foreach (var min in list.minimum[JointNum])
            {
                if (list.kinectSmoothedVelocity[JointNum][min] < T)
                    kinectChart.Series[1].Points.AddXY(list.frameKinectToWii[0][min], list.max[JointNum]);
            }
        }

    }//class
}//namespace