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
        public List<double[,]> kinectPoint = new List<double[,]>();
        public List<double[,]> kinectPointNorm = new List<double[,]>();
        /// <summary>
        /// kinectVelocity[0:元,1:スムージング][時系列順にならんだ速度]
        /// </summary>
        public List<double[]> kinectVec = new List<double[]>();
        public List<double[]> kinectVecSmooth = new List<double[]>();
        public List<double[]> wii = new List<double[]>();
        public List<int>[] frameKinectToWii = new List<int>[2];
        public List<int>[] frameWiiToKinect = new List<int>[2];
        public List<int>[] minimum = new List<int>[21];
        public List<int> bmpNum = new List<int>();

        private VectorBuilder<double> V = Vector<double>.Build;
        private MatrixBuilder<double> M = Matrix<double>.Build;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public List(File file)
        {
            for (int i = 0; i < 2; i++)
            {
                frameKinectToWii[i] = new List<int>();
                frameWiiToKinect[i] = new List<int>();
            }
            for (int i = 0; i < 21; i++) minimum[i] = new List<int>();
            //リストの初期化
            int numOfKinectList = KinectFileToList(file);
            WiiFileToList(file);
            PointToNorm(numOfKinectList);
            PointToVec(numOfKinectList);
        }

        /// <summary>
        /// Kinect.csvからリストを作成
        /// </summary>
        /// <param name="reader"></param>
        private int KinectFileToList(File reader)
        {
            using (var file = reader.InputFile("Kinect.csv"))//ファイルストリームを読み込み
            {
                string line = file.ReadLine();//1行ずつ
                int lineCount = 0;//行数のカウント
                double[,] points = new double[21, 3];//20関節+重心の三次元座標の配列
                while (line != null)
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
                        points[jointNum, i] = double.Parse(token[i + 2]);
                    }
                    if (jointNum == 20) kinectPoint.Add((double[,])points.Clone());//最後の関節を格納したらリストに追加
                }
            }
            return kinectPoint.Count;
        }

        /// <summary>
        /// 正規化した座標のリストを作成
        /// </summary>
        /// <param name="tail"></param>
        private void PointToNorm(int listnum)
        {
            for (int i = 0; i < listnum; i++)
            {
                //体が常に横を向くように正規化
                //HipRightからHipLeftへのびるベクトルに対して垂直なベクトルを計算
                double[] hip = new double[3] { -kinectPoint[i][12, 2] - kinectPoint[i][16, 2],
                                           0,
                                           kinectPoint[i][12, 0] - kinectPoint[i][16, 0]};
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
                double[,] norm = new double[21, 3];
                for (int j = 0; j < 21; j++)
                {
                    var VecJ = V.DenseOfArray(new double[3] { kinectPoint[i][j, 0]-kinectPoint[i][0,0],
                                                             kinectPoint[i][j, 1]-kinectPoint[i][0,1],
                                                             kinectPoint[i][j, 2]-kinectPoint[i][0,2] });
                    var E = mat * VecJ;
                    for (int k = 0; k < 3; k++)
                        norm[j, k] = E.At(j);
                }
                kinectPointNorm.Add((double[,])norm.Clone());
            }
        }

        /// <summary>
        /// 座標のリストから速さのリストを作成
        /// </summary>
        /// <param name="tail"></param>
        private void PointToVec(int listnum)
        {
            kinectVec.Add(new double[21]);//リストの初めは0
            for (int i = 1; i < listnum; i++)
            {
                double[] vec = new double[21];
                for (int j = 0; j < 21; j++)
                {
                    double dx = kinectPoint[i][j, 0] - kinectPoint[i - 1][j, 0];
                    double dy = kinectPoint[i][j, 1] - kinectPoint[i - 1][j, 1];
                    double dz = kinectPoint[i][j, 2] - kinectPoint[i - 1][j, 2];
                    vec[j] = Math.Sqrt(dx * dx + dy * dy + dz * dz);
                }
                kinectVec.Add((double[])vec.Clone());
            }
        }

        private void SmoothVec()
        {

        }

        /// <summary>
        /// Wii.csvからリストを作成
        /// </summary>
        /// <param name="reader"></param>
        private void WiiFileToList(File reader)
        {
            using (var file = reader.InputFile("Wii.csv"))
            {
                string line = file.ReadLine();
                int lineCount = 0;
                double[] accel = new double[3];
                while (line != null)
                {
                    string[] token = line.Split(',');
                    frameWiiToKinect[0].Add(int.Parse(token[0]));//frameListを追加
                    frameWiiToKinect[1].Add(int.Parse(token[1]));//frameListを追加
                    for (int i = 0; i < 3; i++) accel[i] = double.Parse(token[i + 2]);
                    wii.Add((double[])accel.Clone());
                    lineCount++;
                }
            }
        }

    }
}
