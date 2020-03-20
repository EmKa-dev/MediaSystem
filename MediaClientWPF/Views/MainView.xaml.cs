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

            resizer = new WindowResizer(this);
        }
    }
}
