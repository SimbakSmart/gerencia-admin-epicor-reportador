

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModels.Windows
{
    public  class MainWindowViewModel : ViewModelBase
    {
        private Window window;


        public ICommand CloseCommand { get; set; }
        public MainWindowViewModel()
        {
            CloseCommand = new RelayCommand(Close);
        }

        public void Close()
        {
           // window.Close();
            Application.Current.Shutdown();
        }
    }
}
