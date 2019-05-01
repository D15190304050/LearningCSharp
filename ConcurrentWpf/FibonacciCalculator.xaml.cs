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
    /// FibonacciCalculator.xaml 的交互逻辑
    /// </summary>
    public partial class FibonacciCalculator : Window
    {
        /// <summary>
        /// The BackgroundWorker that executes the mission.
        /// </summary>
        private BackgroundWorker worker;

        /// <summary>
        /// The number of Fibonacci to compute.
        /// </summary>
        private int numberToCompute;

        /// <summary>
        /// The highest percentage reached before.
        /// </summary>
        private int highestPercentageReached;

        /// <summary>
        /// Initializes the window for the FibonacciCalculator.
        /// </summary>
        public FibonacciCalculator()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            numberToCompute = 0;
            highestPercentageReached = 0;
        }

        /// <summary>
        /// Set up the BackgroundWorker object by attaching event handlers.
        /// </summary>
        private void InitializeBackgroundWorker()
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
        }

        /// <summary>
        /// Starts to compute the Fibonacci number asynchronously.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdStartAsync_Click(object sender, RoutedEventArgs e)
        {
            // Reset the text in the result TextBox.
            txtResult.Text = "";

            // Disable the TextBox control until the asynchronous operation is done.
            txtIndex.IsEnabled = false;

            // Enable the cancel button until while the asynchronous operation runs.
            cmdCancelAsync.IsEnabled = true;

            // Disable the Start button until the asynchronous operation is done.
            cmdStartAsync.IsEnabled = false;

            // Get the value from the TextBox control.
            numberToCompute = int.Parse(txtIndex.Text);

            // Reset the variable for percentage tracking.
            highestPercentageReached = 0;

            // Start the asynchronous operation.
            worker.RunWorkerAsync(numberToCompute);
        }

        /// <summary>
        /// Cancels the computation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancelAsync_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the asynchronous operation.
            worker.CancelAsync();

            // Disable the Cancel button.
            cmdCancelAsync.IsEnabled = false;
        }

        /// <summary>
        /// The event handler is where the actual, potentially time-consuming work is done.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            if (sender is BackgroundWorker worker)
            {
                // Assign the result of the computation to the Result property of the DoWorkEventArgs object.
                // This will be available to the RunWorkerCompleted event handler.
                e.Result = ComputeFibonacci((int)e.Argument, worker, e);
            }
        }

        /// <summary>
        /// This event handler deals with the results of the background operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled the operation.
                // Note that due to a race condition in the DoWork event handler, the Cancelled may not have been set, even though CancelAsync() was called.
                txtResult.Text = "Cancelled";
            }
            else
            {
                // Finally,  handle the case where the operation succeed.
                txtResult.Text = e.Result.ToString();
            }

            // Enable the TextBox input control.
            txtIndex.IsEnabled = true;

            // Enable the Start button.
            cmdStartAsync.IsEnabled = true;

            // Disable the Cancel button.
            cmdCancelAsync.IsEnabled = false;
        }

        /// <summary>
        /// This event handler updates the progress bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// This is the method that does the actual work. For this example, it computes a Fibonacci number and reports progress as it does its work.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private long ComputeFibonacci(int n, BackgroundWorker worker, DoWorkEventArgs e)
        {
            // The parameter must be >=0 and <= 91.
            // Fib(n) with n > 91 overflows a long.
            if ((n < 0) || (n > 91))
                throw new ArgumentException("Value must be >= 0 and <= 91.");

            long result = 0;

            // Abort the operation if the user has canceled.
            // Note that a call to CancelAsync() may have set CancellationPending to true just after the last invocation of this method exits,
            // so this code will not have the opportunity to set the DoWorkEventArgs.Cancel flag to true.
            // This means that RunWorkerCompletedEventArgs.Cancelled will not be set to true in your RunWorkerCompleted event handler.
            // This is a race condition.
            if (worker.CancellationPending)
                e.Cancel = true;
            else
            {
                if (n < 2)
                    result = 1;
                else
                    result = ComputeFibonacci(n - 1, worker, e) + ComputeFibonacci(n - 2, worker, e);

                // Report progress as a percentage of the total task.
                int percentComplete = (int)((float)n / (float)numberToCompute * 100);
                if (percentComplete > highestPercentageReached)
                {
                    highestPercentageReached = percentComplete;
                    worker.ReportProgress(percentComplete);
                }
            }

            return result;
        }
    }
}
