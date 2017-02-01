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
        /// 作業フォルダへのパス
        /// </summary>
        static string folderPath;
        /// <summary>
        /// bmpフォルダへのパス
        /// </summary>
        static string bmpfolderPath;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public File(String date)
        {
            //date = \\2017...01
            //パスの初期化
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\HoeData\\" + date + "\\";
            bmpfolderPath = folderPath + "bmp\\";
        }

        public StreamReader InputFile(string filename)
        {
            StreamReader file = new StreamReader(folderPath + filename);
            return file;
        }

    }
}
