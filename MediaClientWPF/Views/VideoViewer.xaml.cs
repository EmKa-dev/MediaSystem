using MediaSystem.DesktopClientWPF.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;

namespace MediaSystem.DesktopClientWPF.Views
{
    /// <summary>
    /// Interaction logic for VideoViewer.xaml
    /// </summary>
    public partial class VideoViewer : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isFullScreen;

        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            set {

                _isFullScreen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFullScreen)));
            }
        }

        WindowChrome _originalChromeObject;
        WindowResizer _windowResizer;

        public VideoViewer(Uri videofilepath)
        {
            InitializeComponent();

            MouseDoubleClick += ToggleFullSCreen;

            var vplayer = new VideoPlayerViewModel(videofilepath, this.MyMediaElement);

            this.DataContext = vplayer;

            this.Loaded += (sender, args) => { vplayer.PlayCommand.Execute(sender); };

            MaxHeight = SystemParameters.VirtualScreenHeight;

            _originalChromeObject = WindowChrome.GetWindowChrome(this);

            _windowResizer = new WindowResizer(this);
        }

        private void ToggleFullSCreen(object sender, MouseButtonEventArgs args)
        {
            if (args.OriginalSource is MediaElement)
            {
                if (IsFullScreen)
                {
                    _windowResizer.IncludeTaskBar = true;
                    WindowChrome.SetWindowChrome(this, _originalChromeObject);

                    VideoBorder.SetValue(Grid.RowProperty, 2);
                    VideoBorder.SetValue(Grid.RowSpanProperty, 1);
                    MyMediaElement.SetValue(Grid.RowSpanProperty, 1);
                    this.WindowState = WindowState.Normal;

                    ResizeMode = ResizeMode.CanResize;

                    IsFullScreen = false;
                }
                else
                {
                    _windowResizer.IncludeTaskBar = false;
                    WindowChrome.SetWindowChrome(this, new WindowChrome());

                    VideoBorder.SetValue(Grid.RowProperty, 0);
                    VideoBorder.SetValue(Grid.RowSpanProperty, 3);
                    MyMediaElement.SetValue(Grid.RowSpanProperty, 3);

                    this.WindowState = WindowState.Maximized;

                    ResizeMode = ResizeMode.NoResize;

                    this.Hide();
                    this.Show();

                    IsFullScreen = true;
                }
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            MouseDoubleClick -= ToggleFullSCreen;
            base.OnClosed(e);
        }
    }
}
