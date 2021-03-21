﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BackUpInSynch.Models.ScanStructure;
using BackUpInSynch.Utils;

namespace BackUpInSynch.FormsAndControls.MainForm
{
    public class MainForm : Form
    {
        private readonly DirectoryPanel _folderOne = new DirectoryPanel();
        private readonly DirectoryPanel _folderTwo = new DirectoryPanel();
        private readonly Button _runBtn = new Button();

        public MainForm()
        {
            Controls.Add(_runBtn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            _folderTwo.Top = _folderOne.Bottom + 5;
            Controls.Add(_folderOne);
            Controls.Add(_folderTwo);
            _runBtn.Location = new Point(33 + _folderTwo.Bottom, Width / 2);
            Width = 640;
            _runBtn.Click += RunBtn_Click;
        }


        private void RunBtn_Click(object sender, EventArgs e)
        {
            if (_runBtn.Text == "Results")
            {
                var fc = new ResultsForm.ResultsForm(_folderNodeOne, _folderNodeTwo);
                fc.Show();
                return;
            }

            var pathOne = _folderOne.GetPathIfValid;
            var pathTwo = _folderTwo.GetPathIfValid;
            if (pathOne == null || pathTwo == null)
            {
                MessageBox.Show("Please check paths");
                return;
            }

            _runBtn.Text = "Results";
            _runBtn.Enabled = false;

            BackgroundGenerator.Run((pathOne, pathTwo), DoWork, (a, b) => { _runBtn.Enabled = true; }, null);
        }

        private static DirectoryNode _folderNodeOne;
        private DirectoryNode _folderNodeTwo;

        private void DoWork(object o, DoWorkEventArgs args)
        {
            var (pathOne, pathTwo) = (ValueTuple<string, string>) args.Argument;
            _folderNodeOne = BuildFolderNodesForPath.BuildPath(pathOne, pathOne);
            _folderNodeTwo = BuildFolderNodesForPath.BuildPath(pathTwo, pathTwo);
            MessageBox.Show("Results available");
        }
    }
}