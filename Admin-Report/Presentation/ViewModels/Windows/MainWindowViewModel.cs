using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModels.Windows
{
    public  class MainWindowViewModel : ViewModelBase
    {
       private Window _window;


       
        public ICommand MinimizeCommand { get; private set; }
        public ICommand MaximizeCommand { get; private set; }

        public ICommand CloseCommand { get; private set; }
        public ICommand DragCommand { get; private set; }

        public MainWindowViewModel(Window window)
        {
            _window = window;
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            CloseCommand = new RelayCommand(Close);
            DragCommand = new RelayCommand(Drag);
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
                    // Obtener el área de trabajo disponible en la pantalla
                    var workArea = SystemParameters.WorkArea;

                    // Ajustar la posición y tamaño de la ventana
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

        private void Drag()
        {
            _window.DragMove();
        }


    }
}
