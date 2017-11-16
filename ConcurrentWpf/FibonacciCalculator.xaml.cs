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
    /// FibonacciCalculator.xaml 的交互逻辑
    /// </summary>
    public partial class FibonacciCalculator : Window
    {
        private BackgroundWorker worker;
        private int numberToCompute;
        private int highestPercentageReached;

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
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
        }

        private void cmdStartAsync_Click(object sender, RoutedEventArgs e)
        {

            txtResult.Text = "";
            txtIndex.IsEnabled = false;
            cmdCancelAsync.IsEnabled = true;
            cmdStartAsync.IsEnabled = false;

            numberToCompute = int.Parse(txtIndex.Text);
            highestPercentageReached = 0;
            worker.RunWorkerAsync(numberToCompute);
        }

        private void cmdCancelAsync_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
            cmdCancelAsync.IsEnabled = false;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (sender is BackgroundWorker worker)
            {
                e.Result = ComputeFibonacci((int)e.Argument, worker, e);
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private long ComputeFibonacci(int n, BackgroundWorker worker, DoWorkEventArgs e)
        {
            return 0;
        }


    }
}
