using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoeGraphForm
{
    /// <summary>
    /// ファイルの読み込み・書き込み，各種pathの保存
    /// </summary>
    class File
    {
        /// <summary>
        /// Hoeフォルダ以下へのパス(パスの最後に\\がある)
        /// </summary>
        public string hoePath;
        /// <summary>
        /// Hoeフォルダのパス(パスの最後に\\がない)
        /// </summary>
        public string hoeFolderPath;
        /// <summary>
        /// bmpフォルダ以下へのパス(パスの最後に\\がある)
        /// </summary>
        public string bmpPath;
        /// <summary>
        /// bmpフォルダのパス(パスの最後に\\がない)
        /// </summary>
        public string bmpFolderPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public File(String date)
        {
            //date = \\2017...01
            //パスの初期化
            hoePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\HoeData\\" + date + "\\";
            hoeFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\HoeData\\" + date;
            bmpPath = hoePath + "bmp\\";
            bmpFolderPath = hoePath + "bmp";
        }

        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public StreamReader InputFile(string filename)
        {
            StreamReader file = new StreamReader(hoePath + filename);
            return file;
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="points"></param>
        public void OutputFile(string filename,List<double[]>[] points)
        {
            using (StreamWriter SW = new StreamWriter(hoePath + filename, false))
            {
                for (int line = 0; line < points[0].Count*21; line++)
                {
                    SW.WriteLine(points[line%21][line/21][0] + "," +
                                 points[line % 21][line / 21][1] + "," +
                                 points[line % 21][line / 21][2] );
                }
            }
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="points"></param>
        public void OutputFile(string filename, List<double> velocity, List<int> time)
        {
            using (StreamWriter SW = new StreamWriter(hoePath + filename, false))
            {
                for (int line = 0; line < velocity.Count; line++)
                {
                    SW.WriteLine(time[line] + "," + velocity[line]);
                }
            }
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="points"></param>
        public void OutputFile(string filename, List<double[]> accel, List<int> time)
        {
            using (StreamWriter SW = new StreamWriter(hoePath + filename, false))
            {
                for (int line = 0; line < accel.Count; line++)
                {
                    SW.WriteLine(time[line] + "," + accel[line][3]);
                }
            }
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="points"></param>
        public void OutputFile(string filename, double[] autocorr)
        {
            using (StreamWriter SW = new StreamWriter(hoePath + filename, false))
            {
                for (int line = 0; line < autocorr.Length; line++)
                {
                    SW.WriteLine(line + "," + autocorr[line]);
                }
            }
        }

    }//class
}//
