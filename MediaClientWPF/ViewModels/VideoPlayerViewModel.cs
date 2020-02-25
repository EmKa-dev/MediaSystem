using MediaSystem.Communications;
using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Net;
using System.Threading.Tasks;
using MediaSystem.DesktopClientWPF.Commands;
using MediaSystem.DesktopClientWPF.Models;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class VideoPlayerViewModel : BaseViewModel
    {
        private MediaElement _mediaPlayer;

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

        private IPEndPoint _accessEndPoint;

        public MediaFileInfo CurrentVideoFile { get; set; }

        public ICommand PlayCommand { get; set; }
        public ICommand StopCommand { get; set; }

        #region Constructor

        public VideoPlayerViewModel(MediaFileInfo file, MediaElement mediaPlayer, IPEndPoint endPoint = null)
        {
            this.CurrentVideoFile = file;
            _accessEndPoint = endPoint;

            _mediaPlayer = mediaPlayer;

            _mediaPlayer.LoadedBehavior = MediaState.Manual;
            _mediaPlayer.UnloadedBehavior = MediaState.Manual;

            //Download file from server
            try
            {
                LoadMedia();
            }
            catch (Exception)
            {

                throw;
            }

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

        private async Task LoadMedia()
        {

            //Download file from server
            SessionLogger.LogEvent("Attemtping to download video from server");

            var DLUri = await DownloadFileAsync();

            _mediaPlayer.Source = DLUri;

            PlayFile();
        }

        private Task<Uri> DownloadFileAsync()
        {
            return DownloadService.DownloadFileDataFromServerAsync(CurrentVideoFile, _accessEndPoint);
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
