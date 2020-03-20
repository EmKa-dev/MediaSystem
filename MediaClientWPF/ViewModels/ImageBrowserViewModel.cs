using MediaSystem.Communications;
using System.Net;
using MediaSystem.DesktopClientWPF.Models;
using System.IO;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MediaSystem.DesktopClientWPF.Commands;
using MediaSystem.DesktopClientWPF.Views;
using Microsoft.Extensions.Logging;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class ImageBrowserViewModel : BaseViewModel
    {
        private ILogger _logger;

        private readonly IDownloadService _downloadService;

        public ObservableCollection<byte[]> DownloadedImageFiles { get; set; } = new ObservableCollection<byte[]>();

        public ICommand OpenImageCommand { get; set; }

        public ImageBrowserViewModel(IDownloadService downloadService, ILogger logger)
        {
            _logger = logger;

            _downloadService = downloadService;

            OpenImageCommand = new RelayCommand((imagedata) => new ImageViewer((byte[])imagedata).Show());
        }

        public void InitializeDeviceData(DeviceInfo deviceInfo)
        {
            DownloadAndPopulateCollection(deviceInfo);
            _logger.LogDebug("Finished fetching all image data");
        }

        private void DownloadAndPopulateCollection(DeviceInfo deviceInfo)
        {
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(deviceInfo.ConnectionInfo.IPAddress), deviceInfo.ConnectionInfo.Port);

            foreach (var file in deviceInfo.MediaFiles)
            {
                Uri uri = _downloadService.DownloadFileData(file, iPEnd);
                DownloadedImageFiles.Add(File.ReadAllBytes(uri.AbsolutePath));
            }
        }
    }
}