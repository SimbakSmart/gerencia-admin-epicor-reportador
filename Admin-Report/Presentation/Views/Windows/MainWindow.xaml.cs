using Presentation.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;

namespace Presentation.Views.Windows
{
  
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

        }

       
    }
}
