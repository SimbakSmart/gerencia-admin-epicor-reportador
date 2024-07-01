
using Presentation.Views.Windows;
using System.Windows;

namespace Presentation
{
   
    public partial class App : Application
    {
        public App()
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}
