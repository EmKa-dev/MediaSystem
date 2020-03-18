using MediaSystem.Communications;
using System.Net;
using MediaSystem.DesktopClientWPF.Models;
using System.IO;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using MediaSystem.DesktopClientWPF.Commands;
using MediaSystem.DesktopClientWPF.Views;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class ImageBrowserViewModel : BaseViewModel
    {
        private readonly IDownloadService _downloadService;

        public ObservableCollection<byte[]> DownloadedImageFiles { get; set; } = new ObservableCollection<byte[]>();

        public ICommand OpenImageCommand { get; set; }

        public ImageBrowserViewModel(IDownloadService downloadService)
        {
            _downloadService = downloadService;

            OpenImageCommand = new RelayCommand((imagedata) => new ImageViewer((byte[])imagedata).Show());
        }

        public void InitializeDeviceData(DeviceInfo deviceInfo)
        {
            DownloadAndPopulateCollection(deviceInfo);
            SessionLogger.LogEvent("Finished fetching all image data");
        }

        private void DownloadAndPopulateCollection(DeviceInfo deviceInfo)
        {
            IPEndPoint iPEnd = new IPEndPoint( IPAddress.Parse(deviceInfo.ConnectionInfo.IPAddress), deviceInfo.ConnectionInfo.Port);

            foreach (var file in deviceInfo.MediaFiles)
            {
                Uri uri = _downloadService.DownloadFileData(file, iPEnd);
                DownloadedImageFiles.Add(File.ReadAllBytes(uri.AbsolutePath));
            }
        }
    }
}