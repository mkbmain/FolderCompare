using System;
using System.Windows.Forms;
using BackUpInSynch.FormsAndControls.MainForm;

namespace BackUpInSynch
{
    static class EntryPoint
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}