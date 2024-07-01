using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModels.Windows
{
    public  class MainWindowViewModel : ViewModelBase
    {
       private Window _window;

        public bool isDarkMode;

        public bool IsDarkMode
        {
            get { return isDarkMode; }
            set
            {
                isDarkMode = value;
                RaisePropertyChanged(nameof(IsDarkMode));
            }
        }


        public ICommand MinimizeCommand { get; private set; }
        public ICommand MaximizeCommand { get; private set; }

        public ICommand CloseCommand { get; private set; }

        public ICommand ToggleThemeCommand { get; private set; }
        public MainWindowViewModel(Window window)
        {
            _window = window;

            IsDarkMode =false;

            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            CloseCommand = new RelayCommand(Close);
            ToggleThemeCommand = new RelayCommand(ToggleTheme);
            UpdateTheme();
        }


         private void Minimize()
        {
           _window. WindowState = WindowState.Minimized;
        }
        private void Maximize()
        {
            if (_window != null)
            {
                if (_window.WindowState == WindowState.Maximized)
                {
                    _window.WindowState = WindowState.Normal;
                }
                else
                {
                    var workArea = SystemParameters.WorkArea;

                    _window.Left = workArea.Left;
                    _window.Top = workArea.Top;
                    _window.Width = workArea.Width;
                    _window.Height = workArea.Height;

                    _window.WindowState = WindowState.Maximized;
                }
            }
        }

        private void Close()
        {
            Application.Current.Shutdown();
        }

        private void ToggleTheme()
        {
            IsDarkMode = IsDarkMode ? true : false;
            Properties.Settings.Default.IsDark = IsDarkMode;
            Properties.Settings.Default.Save();
            UpdateTheme();
        }

        private void UpdateTheme()
        {
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
