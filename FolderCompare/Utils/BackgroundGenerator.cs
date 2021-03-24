using System;
using System.ComponentModel;

namespace FolderCompare.Utils
{
    public static class BackgroundGenerator
    {
        public static BackgroundWorker Run(object args, Action<object, DoWorkEventArgs> run,
            Action<object, RunWorkerCompletedEventArgs> whencomplete, Action<object, ProgressChangedEventArgs> progress)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += run.Invoke;
            if (whencomplete != null)
            {
                backgroundWorker.RunWorkerCompleted += whencomplete.Invoke;
            }

            if (progress != null)
            {
                backgroundWorker.ProgressChanged += progress.Invoke;
            }

            backgroundWorker.RunWorkerAsync(args);
            return backgroundWorker;
        }
    }
}