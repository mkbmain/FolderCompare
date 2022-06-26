using System;
using System.Windows.Forms;
using FolderCompare.CalculateMissMatches;
using FolderCompare.Models.ResultCalcModels;

namespace FolderCompare.Utils.Background
{
    public class FolderCompareTaskRunner
    {

        public delegate void UpdateTextHandler(object sender, SetPercentEventArgs e);
        public delegate void DoneHandler(object sender, DoneEventArgs e);
        public event UpdateTextHandler SetProgressBarPercent;
        public event DoneHandler Done;
        public class SetPercentEventArgs : EventArgs
        {
            public int Percent { get; set; }
        }

        public class DoneEventArgs : EventArgs
        {
            public ResultPotentialIssues Issues { get; set; }
        }


        public void DoWork(string pathOne, string pathTwo, bool checkContents)
        {
            try
            {

                var folderNodeOne = BuildFolderNodesForPath.BuildPath(pathOne, pathOne);
                SetProgressBarPercent?.Invoke(this, new SetPercentEventArgs { Percent = checkContents ? 25 : 45 });
                var folderNodeTwo = BuildFolderNodesForPath.BuildPath(pathTwo, pathTwo);
                SetProgressBarPercent?.Invoke(this, new SetPercentEventArgs { Percent = checkContents ? 55 : 85 });
                var issues = CalculateDifferencesDirectories.Issues(folderNodeOne.BasePath, folderNodeTwo.BasePath, folderNodeOne, folderNodeTwo, checkContents);

                Done?.Invoke(this, new DoneEventArgs() { Issues = issues });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                Application.Exit();
            }
        }
    }
}