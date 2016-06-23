//using System;

using System;
using System.Windows.Forms;
using OutlineWF.Views;

namespace OutlineWF
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SerialGraduationForm());
            Application.Run(new DetailEditForm());
            //Application.Run(new EquidistantForm());
        }
    }
}
