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
using System.Windows.Shapes;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Neurotec.Gui
{
    /// <summary>
    /// Interaction logic for BusyForm.xaml
    /// </summary>
    public partial class BusyForm : Window
    {
        private DoWorkEventHandler callback;
        private object argument;
        private RunWorkerCompletedEventArgs results;
        private Exception error;
        protected BackgroundWorker _worker = new BackgroundWorker();

        private BusyForm(string title, DoWorkEventHandler callback)
            : this(title, callback, false, null, null)
        {
        }

        private BusyForm(string title, DoWorkEventHandler callback, object args)
            : this(title, callback, false, args, null)
        {
        }

        private BusyForm(string title, DoWorkEventHandler callback, bool reportsProgress)
            : this(title, callback, reportsProgress, null, null)
        {
        }

        private BusyForm(string title, DoWorkEventHandler callback, bool reportsProgress, object args, EventHandler cancelHandler)
        {
            InitializeComponent();

            //if (!reportsProgress)
            //    progressBar.Style = ProgressBarStyle.Marquee;

            SetExecutionText(title);
            this.callback = callback;
            argument = args;
            _worker.WorkerReportsProgress = reportsProgress;
            _worker.DoWork += BusyForm_DoWork;
            _worker.RunWorkerCompleted += BusyForm_RunWorkerCompleted;
            _worker.ProgressChanged += BusyForm_ProgressChanged;

            //if (cancelHandler != null)
            //{
            //    _worker.WorkerSupportsCancellation = true;
            //    btnCancel.Click += cancelHandler;
            //    btnCancel.Click += new EventHandler(PostOnCancelClick);
            //    btnCancel.Enabled = true;
            //    btnCancel.Visible = true;
            //}
        }

        void PostOnCancelClick(object sender, EventArgs e)
        {
            btnCancel.IsEnabled = false;
            //btnCancel.Refresh();
        }

        public static RunWorkerCompletedEventArgs RunLongTask(string title, DoWorkEventHandler callback)
        {
            return RunLongTask(title, callback, false);
        }

        public static RunWorkerCompletedEventArgs RunLongTask(string title, DoWorkEventHandler callback, bool reportsProgress)
        {
            BusyForm frmLongTask = new BusyForm(title, callback, reportsProgress);
            frmLongTask.ShowDialog();
            if (frmLongTask.error != null)
            {
                throw frmLongTask.error;
            }
            return frmLongTask.results;
        }

        public static RunWorkerCompletedEventArgs RunLongTask(string title, DoWorkEventHandler callback, bool reportsProgress, object args, EventHandler cancelHandler)
        {
            BusyForm frmLongTask = new BusyForm(title, callback, reportsProgress, args, cancelHandler);
            frmLongTask.ShowDialog();
            if (frmLongTask.error != null)
            {
                throw frmLongTask.error;
            }
            return frmLongTask.results;
        }

        private void BusyForm_Shown(object sender, EventArgs e)
        {
            _worker.RunWorkerAsync(argument);
        }

        #region Background worker
        private void BusyForm_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (callback != null)
                {
                    callback(sender, e);
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
        }

        private void BusyForm_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string text = e.UserState as string;
            if (text != null)
            {
                SetExecutionText(text);
            }

            SetExecutionValue(e.ProgressPercentage);
        }

        private void BusyForm_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            results = e;
            Close();
        }
        #endregion

        #region Setters/Getters
        public void SetExecutionText(string text)
        {
            //try
            //{
            //    if (lblOperation.InvokeRequired)
            //    {
            //        lblOperation.Invoke((MethodInvoker)delegate()
            //        {
            //            lblOperation.Text = text;
            //        });
            //    }
            //    else
            //    {
            //        lblOperation.Text = text;
            //    }
            //}
            //finally
            //{
            //}
        }

        public void SetExecutionValue(int value)
        {
            //try
            //{
            //    if (progressBar.InvokeRequired)
            //    {
            //        progressBar.Invoke((MethodInvoker)delegate()
            //        {
            //            progressBar.Value = value;
            //        });
            //    }
            //    else
            //    {
            //        progressBar.Value = value;
            //    }
            //}
            //finally
            //{
            //}
        }
        #endregion

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
