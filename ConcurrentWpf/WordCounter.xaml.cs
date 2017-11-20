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

namespace ConcurrentWpf
{
    /// <summary>
    /// WordCounter.xaml 的交互逻辑
    /// </summary>
    public partial class WordCounter : Window
    {
        private BackgroundWorker worker;

        public WordCounter()
        {
            InitializeComponent();

            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
        }

        /// <summary>
        /// This event is where actual work is done.
        /// This method runs on the background thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker object that raised this event.
            BackgroundWorker worker = (BackgroundWorker)sender;

            // Get the words object and call the background thread.
            Words words = (Words)e.Argument;
            words.CountWords(worker, e);
        }

        /// <summary>
        /// This event handler is called when the background thread finishes.
        /// This method runs on the main thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show("Error:" + e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("Word counting canceled.");
            else
                MessageBox.Show("Mission success.");
        }

        /// <summary>
        /// This method runs on the main thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Words.CurrentState state = (Words.CurrentState)e.UserState;
            txtLinesCounted.Text = state.LinesCounted.ToString();
            txtWordsCounted.Text = state.WordsMatched.ToString();
        }

        /// <summary>
        /// This method runs on the main thread.
        /// </summary>
        private void StartThread()
        {
            txtWordsCounted.Text = "0";
            txtLinesCounted.Text = "0";

            // Initialize the object that the BackgroundWorker calls.
            Words words = new Words();
            words.FilePath = txtFilePath.Text;
            words.WordToCount = txtWord.Text;

            // Start the asynchronous operation.
            worker.RunWorkerAsync(words);
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            StartThread();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the asynchronous operation.
            worker.CancelAsync();
        }
    }
}
