using MediaSystem.Communications;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MediaSystem.DesktopClientWPF.Models
{
    public interface IDownloadService
    {
        Task<Uri> DownloadFileDataAsync(MediaFileInfo file, IPEndPoint iPEnd);

        Uri DownloadFileData(MediaFileInfo file, IPEndPoint iPEnd);
    }
}
