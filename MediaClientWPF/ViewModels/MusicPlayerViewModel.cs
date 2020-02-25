using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Commands;
using MediaSystem.DesktopClientWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class MusicPlayerViewModel : BaseViewModel
    {
        private bool _IsCurrentlyPlaying = false;
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

        private MediaPlayer _mediaPlayer;

        private IPEndPoint _serverEndPoint;

        public ObservableCollection<MediaFileInfo> DownloadedAudioFiles { get; set; } = new ObservableCollection<MediaFileInfo>();

        public ICommand PlayCommand { get; set; }
        public ICommand StopCommand { get; set; }

        #region Constructor

        public MusicPlayerViewModel(List<MediaFileInfo> fileinfos, IPEndPoint ipEnd)
        {
            _serverEndPoint = ipEnd;

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Volume = 1;

            PlayCommand = new RelayCommand((link) => this.PlayDownloadedFile((MediaFileInfo)link));
            StopCommand = new RelayCommand(() => this.StopPlayingFile());


            DownLoadAllMediaToTempFolderAsync(fileinfos);
        }

        private async void DownLoadAllMediaToTempFolderAsync(List<MediaFileInfo> filestodownload)
        {
            //TODO: Poll connection to see if it's still alive.
            foreach (var item in filestodownload)
            {
                await DownloadFileAsync(item);

                DownloadedAudioFiles.Add(item);
            }
       }

        private async Task<Uri> DownloadFileAsync(MediaFileInfo file)
        {
            return await DownloadService.DownloadFileDataFromServerAsync(file, _serverEndPoint);
        }

        #endregion

        private void StopPlayingFile()
        {
            if (IsCurrentlyPlaying)
            {
                _mediaPlayer.Stop();
                IsCurrentlyPlaying = false;
            }
        }

        private void PlayDownloadedFile(MediaFileInfo fileInfo)
        {

            _mediaPlayer.MediaFailed += (o, args) =>
            {
                MessageBox.Show($"Media Failed! {args.ErrorException}");
                
            };

            _mediaPlayer.Open(new Uri($"{DownloadService.DownloadTempFolder}\\{fileInfo.FileName}"));

            _mediaPlayer.Play();

            IsCurrentlyPlaying = true;

        }

        protected override void CloseWindow()
        {
            _mediaPlayer.Close();

            base.CloseWindow();
        }
    }
}
