﻿using Infraesctructure.Interfaces;
using Infraesctructure.Services;
using MaterialDesignThemes.Wpf;
using Presentation.Services;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Presentation.Views.Windows
{

    public partial class SplashWindow : Window
    {
        private ICallsInQueuesProvider _service;
        private bool _isConnected = false;
        private string _message= string.Empty;
        private MessagesRepository localStorate;
        public SplashWindow()
        {
            InitializeComponent();
           _service = new CallsInQueuesProvider();
             localStorate = new MessagesRepository();
           
        }
        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
           // UpdateTheme();
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = _service.TestConnection();

            _isConnected = result.Item1;
            _message = result.Item2;    

            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(70);
            }

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (_isConnected)
            {
                this.Hide();
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
            }
            else
            {
              
            MessageBox.Show(_message,
                            "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
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
