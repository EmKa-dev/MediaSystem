using MediaSystem.Communications;
using System;
using System.Net;

namespace MediaSystem.DesktopClientWPF.Models
{
    public interface IDownloadService
    {
        Uri DownloadFileData(MediaFileInfo file, IPEndPoint iPEnd);
    }
}
