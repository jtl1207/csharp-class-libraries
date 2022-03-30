using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization; 

namespace 手机令牌
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("zh-CN");
            Application.Run(new Form1());
        }
    }
}
