using MediaSystem.DesktopClientWPF;
using MediaSystem.DesktopClientWPF.ViewModels;
using System.Windows;

namespace MediaSystem.DesktopClientWPF.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            this.DataContext = new LoggerViewModel();

            var resizer = new WindowResizer(this);
        }
    }
}
