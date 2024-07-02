using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Presentation.Views.Windows
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            UpdateTheme();
        }
        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(80);
            }

        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           // progressBar.Value = e.ProgressPercentage;

            //if (progressBar.Value == 100)
            //{
            //    MainWindow mainWindow = new MainWindow();
            //    Close();
            //    mainWindow.ShowDialog();
            //}
          
            //MainWindow mainWindow = new MainWindow();
            //this.Close();
            //mainWindow.ShowDialog();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

           this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }

        private void UpdateTheme()
        {
            bool IsDarkMode = false;
            if (Properties.Settings.Default.IsDark)
            {
                IsDarkMode = true;
            }

            PaletteHelper paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(IsDarkMode ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(theme);
        }



    }
}
