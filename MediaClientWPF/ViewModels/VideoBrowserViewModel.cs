using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Commands;
using MediaSystem.DesktopClientWPF.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class VideoBrowserViewModel : BaseViewModel
    {
        private ILogger _logger;

        private readonly IDownloadService _downloadService;

        public ObservableCollection<MediaFileInfo> VideoFilesInfo { get; set; } = new ObservableCollection<MediaFileInfo>();

        public ICommand OpenVideoCommand { get; set; }

        public VideoBrowserViewModel(IDownloadService downloadService, ILogger logger)
        {
            _logger = logger;

            _downloadService = downloadService;

            OpenVideoCommand = new RelayCommand(() => _logger.LogDebug("Videoplayer not implemented yet"));
        }

        public void InitializeDeviceData(DeviceInfo deviceInfo)
        {
            PopulateVideoInfos(deviceInfo.MediaFiles);
        }

        private void PopulateVideoInfos(List<MediaFileInfo> fileInfos)
        {
            foreach (var item in fileInfos)
            {
                VideoFilesInfo.Add(item);
            }
        }
    }
}
