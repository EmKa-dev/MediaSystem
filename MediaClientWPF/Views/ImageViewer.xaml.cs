using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.ViewModels;
using System.Net;
using System.Windows;

namespace MediaSystem.DesktopClientWPF.Views
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        public ImageViewer(MediaFileInfo file, IPEndPoint ipEnd = null)
        {
            InitializeComponent();

            this.DataContext = new ImageViewerViewModel(file, ipEnd);

            var resizer = new WindowResizer(this);
        }
    }
}
