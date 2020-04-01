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

            var vplayer = new VideoPlayerViewModel(videofilepath, this.MyMediaElement);

            this.DataContext = vplayer;

            this.Loaded += (sender, args) => { vplayer.PlayCommand.Execute(sender); }; 

            var resizer = new WindowResizer(this);
        }
    }
}
