using System;
using System.Windows.Input;
using System.Windows.Controls;
using MediaSystem.DesktopClientWPF.Commands;
using Microsoft.Extensions.Logging;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class VideoPlayerViewModel : BaseViewModel
    {
        private MediaElement _mediaPlayer;

        private Uri _videoFileuri;

        private bool _IsCurrentlyPlaying;
        public bool IsCurrentlyPlaying
        {
            get
            {
                return _IsCurrentlyPlaying;
            }

            set
            {
                _IsCurrentlyPlaying = value;
                OnPropertyChanged(this, nameof(IsCurrentlyPlaying));
            }
        }

        public ICommand PlayCommand { get; set; }
        public ICommand StopCommand { get; set; }

        #region Constructor

        public VideoPlayerViewModel(Uri videofile, MediaElement mediaPlayer)
        {
            _mediaPlayer = mediaPlayer;

            _videoFileuri = videofile;

            _mediaPlayer.LoadedBehavior = MediaState.Manual;
            _mediaPlayer.UnloadedBehavior = MediaState.Manual;

            LoadMedia();

            PlayCommand = new RelayCommand(() => this.PlayFile());
            StopCommand = new RelayCommand(() => this.StopPlayingFile());
        }

        private void StopPlayingFile()
        {
            if (IsCurrentlyPlaying)
            {
                _mediaPlayer.Stop();

                IsCurrentlyPlaying = false;
            }
        }

        #endregion

        private void PlayFile()
        {

            if (IsCurrentlyPlaying)
            {
                return;
            }

            _mediaPlayer.Play();

            IsCurrentlyPlaying = true;
        }

        private void LoadMedia()
        {
            _mediaPlayer.Source = _videoFileuri;
        }

        protected override void CloseWindow()
        {
            _mediaPlayer.Close();

            base.CloseWindow();

            //Forces the mediaplayer to release memory
            GC.Collect();
        }
    }
}
