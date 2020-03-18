using DesktopClientWPF;
using MediaSystem.DesktopClientWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MediaSystem.DesktopClientWPF.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly WindowResizer resizer;

        public MainView()
        {
            InitializeComponent();

            DataContext = App.ServiceProvider.GetService<MainViewModel>();

            resizer = new WindowResizer(this);
        }
    }
}
