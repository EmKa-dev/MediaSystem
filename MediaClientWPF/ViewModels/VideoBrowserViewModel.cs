using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Commands;
using MediaSystem.DesktopClientWPF.Models;
using MediaSystem.DesktopClientWPF.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Windows.Input;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class VideoBrowserViewModel : BaseViewModel
    {
        private ILogger _logger;

        private readonly IDownloadService _downloadService;

        private IPEndPoint _deviceEndPoint;

        public ObservableCollection<MediaFileInfo> VideoFilesInfo { get; set; } = new ObservableCollection<MediaFileInfo>();

        public ICommand OpenVideoCommand { get; set; }

        public VideoBrowserViewModel(IDownloadService downloadService, ILogger logger)
        {
            _logger = logger;

            _downloadService = downloadService;

            OpenVideoCommand = new RelayCommand((file) => DownloadVideoAndOpenVideoPlayer((MediaFileInfo)file));
        }

        public void InitializeDeviceData(DeviceInfo deviceInfo)
        {
            _deviceEndPoint = new IPEndPoint(IPAddress.Parse(deviceInfo.ConnectionInfo.IPAddress), deviceInfo.ConnectionInfo.Port);

            PopulateVideoInfos(deviceInfo.MediaFiles);
        }

        private void PopulateVideoInfos(List<MediaFileInfo> fileInfos)
        {
            foreach (var item in fileInfos)
            {
                VideoFilesInfo.Add(item);
            }
        }

        private void DownloadVideoAndOpenVideoPlayer(MediaFileInfo fileinfo)
        {
            Uri filepath = _downloadService.DownloadFileData(fileinfo, _deviceEndPoint);

            var vidplayer = new VideoViewer(filepath);

            vidplayer.Show();
        }
    }
}
