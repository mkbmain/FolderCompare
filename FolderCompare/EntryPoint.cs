using System;
using System.Windows.Forms;
using FolderCompare.FormsAndControls.MainForm;

namespace FolderCompare
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