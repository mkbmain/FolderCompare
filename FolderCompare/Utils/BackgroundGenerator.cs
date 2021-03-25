using System;
using System.ComponentModel;

namespace FolderCompare.Utils
{
    internal static class BackgroundGenerator
    {
        public static BackgroundWorker Run(object args, Action<object, DoWorkEventArgs> run,
            Action<object, RunWorkerCompletedEventArgs> whenComplete, Action<object, ProgressChangedEventArgs> progress)
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += run.Invoke;
            if (whenComplete != null)
            {
                backgroundWorker.RunWorkerCompleted += whenComplete.Invoke;
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