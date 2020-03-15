using MediaSystem.Communications;
using System.Net;
using MediaSystem.DesktopClientWPF.Models;
using System.IO;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class ImageBrowserViewModel : BaseViewModel
    {

        public ObservableCollection<byte[]> DownloadedImageFiles { get; set; } = new ObservableCollection<byte[]>();

        private DeviceInfo _SelectedDevice;
        public DeviceInfo SelectedDevice
        {
            get { return _SelectedDevice; }
            set
            {
                _SelectedDevice = value;
                OnPropertyChanged(this, (nameof(SelectedDevice)));
            }
        }

        public ImageBrowserViewModel(DeviceInfo device)
        {
            SelectedDevice = device;
            DownloadAndPopulateCollection(device.MediaFiles);
            SessionLogger.LogEvent("Finished Downloading all image data from server");
        }

        private void DownloadAndPopulateCollection(List<MediaFileInfo> filestodownload)
        {
            IPEndPoint iPEnd = new IPEndPoint( IPAddress.Parse(SelectedDevice.ConnectionInfo.IPAddress), SelectedDevice.ConnectionInfo.Port);

            foreach (var file in filestodownload)
            {
                Uri uri = DownloadService.DownloadFileDataFromServerSync(file, iPEnd);
                DownloadedImageFiles.Add(File.ReadAllBytes(uri.LocalPath));
            }
        }
    }
}
