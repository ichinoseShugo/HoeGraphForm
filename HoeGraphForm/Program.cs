using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HoeGraphForm
{
    static class Program
    {
        //static string date = "201701181613";
        static string date = "201701181627";
        static int JointNum = 7; //左手

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            File file = new File(date);
            List list = new List(file);
        }
    }
}
