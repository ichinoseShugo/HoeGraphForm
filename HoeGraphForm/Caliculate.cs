using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoeGraphForm
{
    public class Caliculate
    {
        /// <summary>
        /// 自己相関化数を計算
        /// </summary>
        /// <param name="wii"></param>
        /// <returns></returns>
        static public double[] Autocorrelation(List<double[]> wii)
        {
            int N = wii.Count / 2;
            double s;
            double[] R = new double[N];
            for (int j = 0; j < N; j++)
            {
                s = 0;
                for (int i = 1; i < N; i++)
                {
                    s += wii[i][3] * wii[i + j][3];
                }
                R[j] = s;
            }
            for (int j = 0; j < N; j++) R[j] = R[j] / R[0];

            return R;
        }
        /// <summary>
        /// 自己相関関数を計算
        /// </summary>
        /// <param name="velocity"></param>
        /// <returns></returns>
        static public double[] Autocorrelation(List<double> velocity)
        {
            int N = velocity.Count / 2;
            double s;
            double[] R = new double[N];
            for (int j = 0; j < N; j++)
            {
                s = 0;
                for (int i = 1; i < N; i++)
                {
                    s += velocity[i] * velocity[i + j];
                }
                R[j] = s;
            }
            for (int j = 0; j < N; j++) R[j] = R[j] / R[0];

            return R;
        }
        /// <summary>
        /// 基本周波数を計算
        /// </summary>
        /// <returns></returns>
        static public int Frequency(List<double[]> wii)
        {
            int T = 0;
            double[] R = Autocorrelation(wii);
            double maximum = 0;
            bool searchMax = false;
            for (int i = 1; i < R.Length; i++)
            {
                if (searchMax)
                {
                    if (R[i - 1] < R[i] && R[i] > R[i + 1])
                    {
                        if (maximum < R[i])
                        {
                            maximum = R[i];
                            T = i;
                        }
                        else break;
                    }
                }
                if (R[i - 1] < R[i]) searchMax = true;
            }
            return T;
        }
        /// <summary>
        /// 基本周波数を計算
        /// </summary>
        /// <returns></returns>
        static public int Frequency(List<double> velocity)
        {
            int T = 0;
            double[] R = Autocorrelation(velocity);
            double maximum = 0;
            bool searchMax = false;
            for (int i = 1; i < R.Length; i++)
            {
                if (searchMax)
                {
                    if (R[i - 1] < R[i] && R[i] > R[i + 1])
                    {
                        if (maximum < R[i])
                        {
                            maximum = R[i];
                            T = i;
                        }
                        else break;
                    }
                }
                if (R[i - 1] < R[i]) searchMax = true;
            }
            return T;
        }

    }//class
}
