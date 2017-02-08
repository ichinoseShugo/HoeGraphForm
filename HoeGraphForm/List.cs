using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoeGraphForm
{
    /// <summary>
    /// 各種リストを保持
    /// </summary>
    class List
    {
        /// <summary>
        /// kinectPoint[関節番号][順番(時系列)][x:0,y:1,z:2]
        /// </summary>
        public List<double[]>[] kinectPoints = new List<double[]>[21];
        /// <summary>
        /// kinectPoint[関節番号][順番(時系列)][x:0,y:1,z:2]
        /// </summary>
        public List<double[]>[] kinectNormalizedPoints = new List<double[]>[21];
        /// <summary>
        /// Wiiの加速度 wii[順番(時系列順)][x:0,y:1,z:2,三次元ベクトル:3]
        /// </summary>
        public List<double[]> wii = new List<double[]>();
        /// <summary>
        /// kinectVelocity[関節番号][順番(時系列)]
        /// </summary>
        public List<double>[] kinectVelocity = new List<double>[21];
        /// <summary>
        /// 平滑化した速度 kinectSmoothdVelocity[関節番号][順番(時系列)]
        /// </summary>
        public List<double>[] kinectSmoothedVelocity = new List<double>[21];
        /// <summary>
        /// frameKinectToWii[0:Kinect,1:Wii][frame]
        /// Kinectの座標と同じ順番に各座標時のフレームを格納. 1:そのときのWiiのフレーム
        /// </summary>
        public List<int>[] frameKinectToWii = new List<int>[2];
        /// <summary>
        /// frameWiiToKinect[0:Wii,1:Kinect][frame]
        /// Wiiの加速度と同じ順番に各座標時のフレームを格納. 1:そのときのKinectのフレーム
        /// </summary>
        public List<int>[] frameWiiToKinect = new List<int>[2];
        /// <summary>
        /// 極小値のリスト minimum[関節番号][順番(時系列)]
        /// </summary>
        public List<int>[] minimum = new List<int>[21];
        /// <summary>
        /// bmpファイルの番号が格納されるリスト
        /// </summary>
        public List<int> NumOfBmp = new List<int>();
        /// <summary>
        /// 各関節の速さの最大値(平滑化後) max[関節番号]
        /// </summary>
        public double[] max = new double[21];
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public List(File file)
        {
            //配列の初期化
            for (int i = 0; i < 2; i++)
            {
                frameKinectToWii[i] = new List<int>();
                frameWiiToKinect[i] = new List<int>();
            }
            for (int i = 0; i < 21; i++)
            {
                kinectPoints[i] = new List<double[]>();
                kinectNormalizedPoints[i] = new List<double[]>();
                kinectVelocity[i] = new List<double>();
                kinectSmoothedVelocity[i] = new List<double>();
                minimum[i] = new List<int>();
            }
            //リストの初期化
            int numOfKinectList = KinectFileToList(file);
            WiiFileToList(file);
            NormalizePoints(numOfKinectList);
            PointsToVelocity(numOfKinectList);
            SmoothVelocity(numOfKinectList);
            SearchMinimum(numOfKinectList);
            PrepareBmpFile(file.bmpFolderPath);
            //file.OutputFile("Norm.csv", kinectNormalizedPoints);
            //file.OutputFile("SmoothedVelocity_7.csv", kinectSmoothedVelocity[7],frameKinectToWii[0]);
            //file.OutputFile("wii_vec.csv",wii,frameWiiToKinect[0]);
            //file.OutputFile("Autocorrelation_velocity7.csv",Caliculate.Autocorrelation(kinectSmoothedVelocity[7]));
            //file.OutputFile("Autocorrelation_wii_vec.csv", Caliculate.Autocorrelation(wii));
            //Console.WriteLine("wii:"+Caliculate.Freqency(wii)+"vel7:"+ Caliculate.Freqency(kinectSmoothedVelocity[7]));
        }

        /// <summary>
        /// Kinect.csvからリストを作成
        /// </summary>
        /// <param name="reader"></param>
        private int KinectFileToList(File reader)
        {
            int lineCount = 0;//行数のカウント
            using (var file = reader.InputFile("Kinect.csv"))//ファイルストリームを読み込み
            {
                string line;//1行ずつ
                double[] points = new double[3];//三次元座標の配列
                while ((line = file.ReadLine()) != null)
                {
                    string[] token = line.Split(',');
                    int jointNum = lineCount++ % 21;
                    if (jointNum == 0)//frameListを追加
                    {
                        frameKinectToWii[0].Add(int.Parse(token[0]));
                        frameKinectToWii[1].Add(int.Parse(token[1]));
                    }
                    for (int i = 0; i < 3; i++)//配列に三次元座標を格納
                    {
                        points[i] = double.Parse(token[i + 2]);
                    }
                    kinectPoints[jointNum].Add((double[])points.Clone());
                }
            }
            return lineCount/21;
        }

        /// <summary>
        /// Wii.csvからリストを作成
        /// </summary>
        /// <param name="reader"></param>
        private void WiiFileToList(File reader)
        {
            using (var file = reader.InputFile("Wii.csv"))
            {
                string line;
                double[] accel = new double[4];//各軸加速度と三次元加速度を格納する三次元配列
                while ((line = file.ReadLine()) != null)
                {
                    string[] token = line.Split(',');
                    frameWiiToKinect[0].Add(int.Parse(token[0]));//frameListを追加
                    frameWiiToKinect[1].Add(int.Parse(token[1]));//frameListを追加
                    for (int i = 0; i < 3; i++) accel[i] = double.Parse(token[i + 2]);//各軸における加速度を格納
                    accel[3] = Math.Sqrt(accel[0]* accel[0] + accel[1]* accel[1] + accel[2]* accel[2]);//三次元加速度を格納
                    wii.Add((double[])accel.Clone());//リストに追加
                }
            }
        }

        /// <summary>
        /// 正規化した座標のリストを作成
        /// </summary>
        /// <param name="tail"></param>
        private void NormalizePoints(int listnum)
        {
            VectorBuilder<double> Vector = Vector<double>.Build;
            MatrixBuilder<double> Matrix = Matrix<double>.Build;
            for (int i = 0; i < listnum; i++)
            {
                //体が常に横を向くように正規化
                //HipRightからHipLeftへのびるベクトルに対して垂直なベクトルを計算
                double[] hip = new double[3] { -kinectPoints[12][i][2] - kinectPoints[16][i][2],
                                           0,
                                           kinectPoints[12][i][0] - kinectPoints[16][i][0]};
                var VecHip = Vector.DenseOfArray(hip);
                //求めたベクトルの絶対値を1に
                var e_x = VecHip.Divide((float)VecHip.L2Norm());
                //e_y = (0,1,0)
                var e_y = Vector.DenseOfArray(new double[3] { 0, 1, 0 });
                //e_z = e_x × e_y
                var e_z = Vector.DenseOfArray(new double[3] { -VecHip.At(2), 0, VecHip.At(0) });//1
                                                                                                //求めたベクトルの絶対値を1に
                e_z = e_z.Divide((float)e_z.L2Norm());

                //e_x,e_y,e_zを一つの行列に
                var mat = Matrix.DenseOfColumnVectors(new Vector<double>[] { e_x, e_y, e_z }).Inverse();
                double[] norm = new double[3];//正規化した座標を格納する三次元座標
                for (int j = 0; j < 21; j++)
                {
                    var VecJ = Vector.DenseOfArray(new double[3] { kinectPoints[j][i][0]-kinectPoints[0][i][0],
                                                             kinectPoints[j][i][1]-kinectPoints[0][i][1],
                                                             kinectPoints[j][i][2]-kinectPoints[0][i][2]});
                    var E = mat * VecJ;
                    for (int k = 0; k < 3; k++)
                        norm[k] = E.At(k);
                    kinectNormalizedPoints[j].Add((double[])norm.Clone());
                }
                //頭の高さを1として正規化
                double[] head = kinectNormalizedPoints[3][i];
                double headVec = Math.Sqrt(head[0] * head[0] + head[1] * head[1] + head[2] * head[2]);
                for (int j = 0; j < 21; j++)//jointnum:3 = head
                {
                    double[] points = kinectNormalizedPoints[j][i];
                    double Vec = Math.Sqrt(points[0] * points[0] + points[1] * points[1] + points[2] * points[2]);
                    Vec /= headVec;
                    kinectNormalizedPoints[j][i][0] *= Vec;
                }
            }
        }

        /// <summary>
        /// 座標のリストから速さのリストを作成
        /// </summary>
        /// <param name="tail"></param>
        private void PointsToVelocity(int listnum)
        {
            for(int i=0; i<21; i++) kinectVelocity[i].Add(0);//最初の速さは0
            for (int i = 1; i < listnum; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    double dx = kinectPoints[j][i][0] - kinectPoints[j][i - 1][0];
                    double dy = kinectPoints[j][i][1] - kinectPoints[j][i - 1][1];
                    double dz = kinectPoints[j][i][2] - kinectPoints[j][i - 1][2];
                    kinectVelocity[j].Add(Math.Sqrt(dx * dx + dy * dy + dz * dz));
                }
            }
        }

        /// <summary>
        /// スムージングした速さのリストを作成
        /// </summary>
        /// <param name="listnum"></param>
        private void SmoothVelocity(int listnum)
        {
            //for (int i = 0; i < 21; i++) kinectSmoothedVelocity[i].Add(0);//最初の速さは0
            double Y = 0;
            for (int i = 0; i < listnum; i++)//行
            {
                for (int j = 0; j < 21; j++)//関節
                {
                    if (i < 2)//i = 0, i = 1
                    {
                        for (int k = 0; k < i + 2; k++) Y += kinectVelocity[j][k];
                        kinectSmoothedVelocity[j].Add(Y / (i + 3) );//012,0123
                        Y = 0;
                    }
                    else if (i > listnum - 3)//i = listnum-1, i = listnum-2
                    {
                        for (int k = listnum - 1; k < i - 2; k--) Y += kinectVelocity[j][k];
                        kinectSmoothedVelocity[j].Add(Y / (3 + (listnum - 1 - i)) );//listnum-3listnum-2listnum-1,listnum-4listnum-3listnum-2listnum-1
                        Y = 0;
                    }
                    else
                    {
                        for (int k = 0; k < 5; k++) Y += kinectVelocity[j][i + k - 2];
                        kinectSmoothedVelocity[j].Add(Y / 5);
                        Y = 0;
                    }
                    if (max[j] < kinectSmoothedVelocity[j][i]) max[j] = kinectSmoothedVelocity[j][i];//最大値を更新
                }
            }
        }

        /// <summary>
        /// 極小値を探す
        /// </summary>
        private void SearchMinimum(int listnum)
        {
            for (int i = 0; i < 21; i++)
            {
                for (int j = 1; j < listnum-1; j++)
                {
                    if (kinectSmoothedVelocity[i][j-1] > kinectSmoothedVelocity[i][j] &&
                       kinectSmoothedVelocity[i][j] > kinectSmoothedVelocity[i][j+1])
                        minimum[i].Add(j);
                }
            }
        }

        /// <summary>
        /// パスからbmpファイルの名前を取得しリストにする
        /// </summary>
        private void PrepareBmpFile(string folder)
        {
            string[] files = System.IO.Directory.GetFiles(folder, "*");
            foreach (var file in files)
            {
                //bmpファイルのパスからbmp番号をtoken2[0]に格納
                string[] token1 = file.Split('\\');
                string[] token2 = token1[7].Split('.');
                //bmpの番号をリストに追加
                NumOfBmp.Add(int.Parse(token2[0]));
            }
            //bmpの番号順にソート
            NumOfBmp.Sort();
        }

    }//class
}//namespace
