using MediaSystem.DesktopClientWPF.ViewModels;
using System;
using System.Windows;

namespace MediaSystem.DesktopClientWPF.Views
{
    /// <summary>
    /// Interaction logic for VideoViewer.xaml
    /// </summary>
    public partial class VideoViewer : Window
    {
        public VideoViewer(Uri videofilepath)
        {
            InitializeComponent();

            this.DataContext = new VideoPlayerViewModel(videofilepath, this.MyMediaElement);

            var resizer = new WindowResizer(this);
        }
    }
}
