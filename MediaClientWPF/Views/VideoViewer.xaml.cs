using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shell;
using System.Windows.Threading;

namespace MediaSystem.DesktopClientWPF.Views
{
    /// <summary>
    /// Interaction logic for VideoViewer.xaml
    /// </summary>
    public partial class VideoViewer : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        WindowChrome _originalChromeObject;
        WindowResizer _windowResizer;
        private DispatcherTimer _timer;

        private bool _isFullScreen;
        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            set {

                _isFullScreen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFullScreen)));
            }
        }

        private bool _isDragging = false;
        public bool IsDragging
        {
            get { return _isDragging; }
            set
            {

                _isDragging = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDragging)));
            }
        }


        public VideoViewer(Uri videofilepath)
        {
            InitializeComponent();

            //Setup mediaelement/Seekbar
            MyMediaElement.LoadedBehavior = MediaState.Manual;
            MyMediaElement.UnloadedBehavior = MediaState.Manual;
            MyMediaElement.Source = videofilepath;
            MyMediaElement.MediaOpened += (o, args) =>
            {
                MySeekBar.Minimum = 0;
                MySeekBar.Maximum = MyMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                MySeekBar.SmallChange = 1;
                MySeekBar.Value = MyMediaElement.Position.TotalSeconds;
                TotalDurationText.Text = MyMediaElement.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

                // Add handlers for preview events so we can set IsDragging to true while we click on the track,
                // to give us a chance to manually update media-position.
                // If we don't, the slider will jump back to it's previous position while it updates itself to keep track of current media-position.
                // Another reason this is needed is because most mouse events are handled by the thumb-control before it reaches the track.
                // This allows the event to be handled even if it was already handled by some other control.
                MySeekBar.AddHandler(Slider.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MySeekBar_PreviewMouseLeftButtonDown), true);
                MySeekBar.AddHandler(Slider.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(MySeekBar_PreviewMouseLeftButtonUp), true);
            };


            //Setup timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += UpdateSeekBar;
            _timer.Start();

            MouseDoubleClick += ToggleFullSCreen;

            //Setup view-specific

            //To allow the full height of the screen to be used for fullscreen
            MaxHeight = SystemParameters.VirtualScreenHeight;

            //For fullscreen to work correctly we need to set WindowChrome to a new empty WindowChrome-object when entering fullscreen
            //Save the original WindowChrome so we can reapply it when exiting fullscreen
            _originalChromeObject = WindowChrome.GetWindowChrome(this);
            _windowResizer = new WindowResizer(this);

            MyMediaElement.Play();
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
            MyMediaElement.Close();
            MouseDoubleClick -= ToggleFullSCreen;
            base.OnClosed(e);
        }

        private void MySeekBar_DragStarted(object sender, DragStartedEventArgs e)
        {
            IsDragging = true;
            MyMediaElement.Pause();
        }

        private void MySeekBar_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            IsDragging = false;
            UpdateMediaPosition((int)MySeekBar.Value);
            MyMediaElement.Play();
        }

        private void UpdateSeekBar(object sender, EventArgs args)
        {
            if (!IsDragging)
            {
                CurrentTimeText.Text = MyMediaElement.Position.ToString(@"mm\:ss");
                MySeekBar.Value = MyMediaElement.Position.TotalSeconds;
            }        
        }

        private void UpdateMediaPosition(double pos)
        {
            MyMediaElement.Position = new TimeSpan(0, 0, 0, (int)pos);
        }

        private void MySeekBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDragging = true;
        }

        private void MySeekBar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateMediaPosition(MySeekBar.Value);
            IsDragging = false;
        }

        private void Button_Play_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Play();
        }

        private void Button_Pause_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Pause();
        }

        private void Button_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
