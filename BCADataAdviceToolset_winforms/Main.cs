#region License, Terms and Conditions
//
// BCAAImport: BC Assessment Import Utility
// Written by Colin Dyck (Selkirk Geospatial Research Centre)
// Copyright (c) 2012. All rights reserved.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License
// along with this library; If not, see <http://www.gnu.org/licenses/>.
//
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using log4net;
using log4net.Appender;

namespace SGRC.BCATools
{
    /// <summary>
    /// Main form
    /// </summary>
    public partial class Main : Form, IAppender
    {
        BackgroundWorker _backgroundWorker;
        public delegate void LogItem(log4net.Core.LoggingEvent loggingEvent);
        public LogItem _logItemHandler;
        bool warningOccured;

        /// <summary>
        /// Initializes a new instance of the <see cref="Main" /> class.
        /// </summary>
        public Main()
        {
            InitializeComponent();

            //init txt values from settings if exists
            if (!string.IsNullOrEmpty(Properties.Settings.Default.SourceDatFile))
            {
                txtSourceDatFile.Text = Properties.Settings.Default.SourceDatFile;
                BCASession.Current.SourceDatFile = Properties.Settings.Default.SourceDatFile;
            }

            if (!string.IsNullOrEmpty(Properties.Settings.Default.DestinationDBFile))
            {
                txtDestinationDBFile.Text = Properties.Settings.Default.DestinationDBFile;
                BCASession.Current.DestinationDb = Properties.Settings.Default.DestinationDBFile;
            }

            _logItemHandler = new LogItem(LogItemMethod);
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.WorkerReportsProgress = false;
            _backgroundWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs" /> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //save settings
            Properties.Settings.Default.SourceDatFile = txtSourceDatFile.Text;
            Properties.Settings.Default.DestinationDBFile = txtDestinationDBFile.Text;
            Properties.Settings.Default.Save();

            base.OnFormClosing(e);
        }
        /// <summary>
        /// Handles the RunWorkerCompleted event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;

            //notify user
            FileAppender file = BCASession.Current.Log.Logger.Repository.GetAppenders().FirstOrDefault(a => a.Name == "GeneralLog") as FileAppender;
            string logDesc = file != null ? System.IO.Path.GetFileName(file.File) : "log";

            //see if error occured
            if (e.Error != null)
            {
                BCASession.Current.Log.Error(e.Error.Message);
                BCASession.Current.Log.Error(e.Error.StackTrace);
                txtLog.Text = string.Format("Error occured; please see {0} for details", logDesc);
                return;
            }

            if (warningOccured)
            {
                txtLog.Text = string.Format("Process completed with warning(s) occured; please see {0} for details", logDesc);
            }
            else
            {
                txtLog.Text = string.Format("Process completed successfully; please see {0} for details", logDesc);
            }
        }

        /// <summary>
        /// Handles the DoWork event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            warningOccured = false;

            switch (e.Argument.ToString())
            {
                case "year":
                    ImportYearDatFile();
                    break;

                case "month":
                    ImportMonthDatFile();
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnChooseSourceDatPath control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnChooseSourceDatPath_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "dat files (*.dat)|*.dat";
            if (System.IO.File.Exists(BCASession.Current.SourceDatFile))
            {
                openFileDialog1.InitialDirectory = System.IO.Directory.GetParent(BCASession.Current.SourceDatFile).FullName;
            }
            else
            {
                openFileDialog1.InitialDirectory = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            }

            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtSourceDatFile.Text = openFileDialog1.FileName;
                BCASession.Current.SourceDatFile = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnChooseDestinationDBFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnChooseDestinationDBFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "MS Access files (*.mdb)|*.mdb";

            if (System.IO.File.Exists(BCASession.Current.DestinationDb))
            {
                openFileDialog1.InitialDirectory = System.IO.Directory.GetParent(BCASession.Current.DestinationDb).FullName;
            }
            else
            {
                openFileDialog1.InitialDirectory = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            }

            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtDestinationDBFile.Text = openFileDialog1.FileName;
                BCASession.Current.DestinationDb = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnImportMonth control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnImportMonth_Click(object sender, EventArgs e)
        {
            if (_backgroundWorker.IsBusy)
            {
                MessageBox.Show("There is an import process currently running...please wait until complete to start another");
                return;
            }

            if (ValidateSettings())
            {
                if (MessageBox.Show(string.Format("{0} will be imported to {1}.  Press OK to continue.",
                    System.IO.Path.GetFileName(BCASession.Current.SourceDatFile),
                    System.IO.Path.GetFileName(BCASession.Current.DestinationDb)), "Import DAT", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    _backgroundWorker.RunWorkerAsync("month");
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnImportYear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnImportYear_Click(object sender, EventArgs e)
        {
            if (_backgroundWorker.IsBusy)
            {
                MessageBox.Show("There is an import process currently running...please wait until complete to start another");
                return;
            }

            if (ValidateSettings())
            {
                if (MessageBox.Show(string.Format("{0} will be imported to {1}.  Press OK to continue.",
                    System.IO.Path.GetFileName(BCASession.Current.SourceDatFile),
                    System.IO.Path.GetFileName(BCASession.Current.DestinationDb)), "Import DAT", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    _backgroundWorker.RunWorkerAsync("year");
                }
            }
        }

        /// <summary>
        /// Imports the year dat file.
        /// </summary>
        private void ImportYearDatFile()
        {
            ConfigureLog();
            BCASession.Current.Log.Info(string.Format("Beginning Yearly file import from {0}...", BCASession.Current.SourceDatFile));

            //create the iterator to build all the business objects
            BCAObjectLoader loader = new BCAObjectLoader(BCASession.Current.SourceDatFile);
            loader.Start();

            BCAPersister persist = new BCAPersister();
            persist.PersistYearlyDatFile(loader);
        }

        /// <summary>
        /// Imports the month dat file.
        /// </summary>
        private void ImportMonthDatFile()
        {
            //log name contains import dat name and date
            ConfigureLog();
            BCASession.Current.Log.Info(string.Format("Beginning Monthly file import from {0}...", BCASession.Current.SourceDatFile));

            //create the iterator to build all the business objects
            BCAObjectLoader loader = new BCAObjectLoader(BCASession.Current.SourceDatFile);
            loader.Start();

            BCAPersister persist = new BCAPersister();
            persist.PersistMonthlyDatFile(loader);
        }

        /// <summary>
        /// Handles the Click event of the btnAbout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        /// <summary>
        /// Handles the TextChanged event of the txtSourceDatFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void txtSourceDatFile_TextChanged(object sender, EventArgs e)
        {
            BCASession.Current.SourceDatFile = txtSourceDatFile.Text;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtDestinationDBFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void txtDestinationDBFile_TextChanged(object sender, EventArgs e)
        {
            BCASession.Current.DestinationDb = txtDestinationDBFile.Text;
        }

        private void ConfigureLog()
        {
            //log name contains import dat name and date
            BCASession.Current.ConfigureLog(string.Format("{0}_{1}_{2:dd.MM.yyy hh.mm.ss}",
                System.IO.Path.GetFileNameWithoutExtension(BCASession.Current.DestinationDb),
                System.IO.Path.GetFileNameWithoutExtension(BCASession.Current.SourceDatFile),
                DateTime.Now));
            ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root.AddAppender(this);
        }

        /// <summary>
        /// Logs the item method.
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        public void LogItemMethod(log4net.Core.LoggingEvent loggingEvent)
        {
            if (loggingEvent.Level == log4net.Core.Level.Info)
            {
                txtLog.Text = loggingEvent.MessageObject.ToString();
            }
            else if (loggingEvent.Level == log4net.Core.Level.Warn)
            {
                //just set a flag
                warningOccured = true;
            }
        }

        /// <summary>
        /// Log the logging event in Appender specific way.
        /// </summary>
        /// <param name="loggingEvent">The event to log</param>
        /// <remarks>
        /// This method is called to log a message into this appender.
        /// </remarks>
        public void DoAppend(log4net.Core.LoggingEvent loggingEvent)
        {
            if (!txtLog.InvokeRequired || !txtLog.IsHandleCreated)
            {
                return;
            }
            txtLog.Invoke(_logItemHandler, loggingEvent);
        }

        /// <summary>
        /// Validates the settings.
        /// </summary>
        /// <returns></returns>
        private bool ValidateSettings()
        {
            if (string.IsNullOrEmpty(BCASession.Current.SourceDatFile) || !System.IO.File.Exists(BCASession.Current.SourceDatFile))
            {
                MessageBox.Show("Invalid Source Dat File.  Processing cannot continue");
                return false;
            }

            if (string.IsNullOrEmpty(BCASession.Current.DestinationDb) || !System.IO.File.Exists(BCASession.Current.DestinationDb))
            {
                MessageBox.Show("Invalid Destination Db file.  Processing cannot continue");
                return false;
            }

            return true;
        }
    }
}
