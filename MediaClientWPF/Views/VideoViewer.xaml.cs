using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF;
using MediaSystem.DesktopClientWPF.ViewModels;
using System.Net;
using System.Windows;

namespace MediaSystem.DesktopClientWPF.Views
{
    /// <summary>
    /// Interaction logic for VideoViewer.xaml
    /// </summary>
    public partial class VideoViewer : Window
    {
        public VideoViewer(MediaFileInfo file, IPEndPoint ipEnd = null)
        {
            InitializeComponent();

            this.DataContext = new VideoPlayerViewModel(file, this.MyMediaElement, ipEnd);

            var resizer = new WindowResizer(this);
        }
    }
}
