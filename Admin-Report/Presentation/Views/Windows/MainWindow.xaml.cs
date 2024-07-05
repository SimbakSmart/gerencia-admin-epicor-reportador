using Microsoft.Win32;
using Notifications.Wpf;
using Presentation.Helpers;
using Presentation.Utils;
using Presentation.ViewModels.Windows;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace Presentation.Views.Windows
{

    public partial class MainWindow : Window
    {
        private const int SPI_GETSCREENSAVERRUNNING = 114;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(int uiAction, int uiParam, ref bool pvParam, int fWinIni);


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
           SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            //switch (e.Reason)
            //{
            //    case SessionSwitchReason.SessionLock:
            //        // Screen saver activated or desktop locked
            //        MessageBox.Show("Screen saver or desktop lock detected. Closing application.");
            //        Application.Current.Shutdown();
            //        break;
            //    case SessionSwitchReason.SessionUnlock:
            //        // Desktop unlocked
            //        MessageBox.Show("Desktop unlocked.");
            //        break;
            //    case SessionSwitchReason.SessionLogon:
            //        // User logged on
            //        MessageBox.Show("User logged on.");
            //        break;
            //    case SessionSwitchReason.SessionLogoff:
            //        // User logged off
            //        MessageBox.Show("User logged off.");
            //        break;
            //}
            Application.Current.Shutdown();
            // MessageBox.Show("El aplicativo fue cerrado.");
            //NotifiactionMessage
            //   .SetMessage("Información", "Se libero el aplicativo por motivos de rendimiento",
            //           NotificationType.Success);


        }

        private bool IsScreenSaverRunning()
        {
            bool isRunning = false;
            SystemParametersInfo(SPI_GETSCREENSAVERRUNNING, 0, ref isRunning, 0);
            return isRunning;
        }

        protected override void OnClosed(EventArgs e)
        {
            // Ensure to unsubscribe from events to avoid memory leaks
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            base.OnClosed(e);
        }

    }
}
