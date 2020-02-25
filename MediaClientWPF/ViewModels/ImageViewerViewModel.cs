using MediaSystem.Communications;
using System.Net;
using MediaSystem.DesktopClientWPF.Models;
using System.IO;
using System;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class ImageViewerViewModel : BaseViewModel
    {
        public MediaFileInfo FileInfo { get; set; }

        public byte[] ImageData { get; set; }

        public ImageViewerViewModel(MediaFileInfo file, IPEndPoint ipEnd = null)
        {
            FileInfo = file;

            //TODO, make this not blocking
            Uri uri = DownloadService.DownloadFileDataFromServerSync(file, ipEnd);
            ImageData = File.ReadAllBytes(uri.LocalPath);
            SessionLogger.LogEvent("Downloaded image data from server");
        }
    }
}
