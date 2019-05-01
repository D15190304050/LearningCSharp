using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Concurrent
{
    /// <summary>
    /// ProgressPercentage.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressPercentage : Window
    {
        BackgroundWorker worker;

        public ProgressPercentage()
        {
            InitializeComponent();
            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            // Start the asynchronous operation if the BackgroundWorker is not busy.
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the asynchronous operation if it supports cancellation.
            if (worker.WorkerSupportsCancellation)
                worker.CancelAsync();
        }

        /// <summary>
        /// This event handler is where the time-consuming work is done.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (sender is BackgroundWorker worker)
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                    {
                        // Perform a time consuming operation and report progress.
                        System.Threading.Thread.Sleep(500);
                        worker.ReportProgress(i * 10);
                    }
                }
            }
        }

        /// <summary>
        /// This event handler updates the progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            txtProgress.Text = e.ProgressPercentage + "%";
        }

        /// <summary>
        /// This event handler deal with the results of the background operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                txtProgress.Text = "Canceled!";
            else if (e.Error != null)
                txtProgress.Text = "Error: " + e.Error.Message;
            else
                txtProgress.Text = "Done";
        }
    }
}
