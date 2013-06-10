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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;

using log4net;
using log4net.Appender;


namespace SGRC.BCATools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker _backgroundWorker;
        public delegate void LogItem(log4net.Core.LoggingEvent loggingEvent);
        public LogItem _logItemHandler;
        bool warningOccured;

        public MainWindow()
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

            _logItemHandler = new LogItem(LogEvent);
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.WorkerReportsProgress = false;
            _backgroundWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            //show splash screen for a bit
            System.Threading.Thread.Sleep(1600);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //save settings
            Properties.Settings.Default.SourceDatFile = txtSourceDatFile.Text;
            Properties.Settings.Default.DestinationDBFile = txtDestinationDBFile.Text;
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs" /> instance containing the event data.</param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Arrow;

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
                    System.IO.Path.GetFileName(BCASession.Current.DestinationDb)), "Import DAT", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    this.Cursor = Cursors.Wait;
                    _backgroundWorker.RunWorkerAsync("month");
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

        private void ConfigureLog()
        {
            //log name contains import dat name and date
            BCASession.Current.ConfigureLog(string.Format("{0}_{1}_{2:dd.MM.yyy hh.mm.ss}",
                System.IO.Path.GetFileNameWithoutExtension(BCASession.Current.DestinationDb),
                System.IO.Path.GetFileNameWithoutExtension(BCASession.Current.SourceDatFile),
                DateTime.Now));

            //do the delegater for showing log in txtLog
            foreach (log4net.Appender.IAppender appender in log4net.LogManager.GetRepository().GetAppenders())
            {
                if (appender.GetType() == typeof(DelegateAppender))
                {
                    DelegateAppender delegateAppender = (DelegateAppender)appender;
                    delegateAppender.OnEventLogged = HandleLoggingEvent;
                    return;
                }
            }

        }

        /// <summary>
        /// Log the logging event in Appender specific way.
        /// </summary>
        /// <param name="loggingEvent">The event to log</param>
        /// <remarks>
        /// This method is called to log a message into this appender.
        /// </remarks>
        public void HandleLoggingEvent(log4net.Core.LoggingEvent loggingEvent)
        {
            if (txtLog.Dispatcher.CheckAccess())
            {
                // The calling thread owns the dispatcher, and hence the UI element
                LogEvent(loggingEvent);
            }
            else
            {
                // Invokation required
                txtLog.Dispatcher.Invoke(_logItemHandler, loggingEvent);
            }
        }

        /// <summary>
        /// Logs the item method.
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        public void LogEvent(log4net.Core.LoggingEvent loggingEvent)
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

        /// <summary>
        /// Handles the Click event of the btnImportYear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void btnImportYear_Click(object sender, RoutedEventArgs e)
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
                    System.IO.Path.GetFileName(BCASession.Current.DestinationDb)), "Import DAT", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    this.Cursor = Cursors.Wait;
                    _backgroundWorker.RunWorkerAsync("year");
                }
            }

        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the btnChooseSourceDatFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void btnChooseSourceDatFile_Click(object sender, RoutedEventArgs e)
        {
            ChooseSource();
        }

        /// <summary>
        /// Handles the Click event of the btnChooseDestinationDbFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void btnChooseDestinationDbFile_Click(object sender, RoutedEventArgs e)
        {
            ChooseDestination();
        }

        /// <summary>
        /// Handles the PreviewMouseLeftButtonDown event of the txtSourceDatFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void txtSourceDatFile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChooseSource();
        }

        /// <summary>
        /// Handles the PreviewMouseLeftButtonDown event of the txtDestinationDBFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" /> instance containing the event data.</param>
        private void txtDestinationDBFile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChooseDestination();
        }

        /// <summary>
        /// Chooses the source.
        /// </summary>
        private void ChooseSource()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "dat files (*.dat)|*.dat";
            if (System.IO.File.Exists(BCASession.Current.SourceDatFile))
            {
                dlg.InitialDirectory = System.IO.Directory.GetParent(BCASession.Current.SourceDatFile).FullName;
            }
            else
            {
                dlg.InitialDirectory = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            }

            bool? result = dlg.ShowDialog(this);
            if (result == true)
            {
                txtSourceDatFile.Text = dlg.FileName;
                BCASession.Current.SourceDatFile = dlg.FileName;
            }

        }

        /// <summary>
        /// Chooses the destination.
        /// </summary>
        private void ChooseDestination()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "MS Access files (*.mdb)|*.mdb";

            if (System.IO.File.Exists(BCASession.Current.DestinationDb))
            {
                dlg.InitialDirectory = System.IO.Directory.GetParent(BCASession.Current.DestinationDb).FullName;
            }
            else
            {
                dlg.InitialDirectory = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            }

            bool? result = dlg.ShowDialog(this);
            if (result == true)
            {
                txtDestinationDBFile.Text = dlg.FileName;
                BCASession.Current.DestinationDb = dlg.FileName;
            }
        }

    }
}
