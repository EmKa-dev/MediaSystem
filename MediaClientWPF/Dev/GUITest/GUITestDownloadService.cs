using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MediaSystem.DesktopClientWPF.Dev.GUITest
{
    public class GUITestDownloadService : IDownloadService, IDisposable
    {
        private ILogger _logger;

        private readonly string _downloadTempFolder;

        private List<Uri> _resourceimagedata = new List<Uri>();

        public GUITestDownloadService(ILogger logger)
        {
            _logger = logger;
            _downloadTempFolder = GetTempFolder();
            CreateTestImages();
        }

        private void CreateTestImages()
        {
            var images = TestImageResources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            foreach (DictionaryEntry item in images)
            {

                byte[] b = item.Value as byte[];

                string filepath = $"{_downloadTempFolder}/{item.Key.ToString()}.png";
                var fs = File.Create(filepath);
                fs.Write(b);
                fs.Close();

                _resourceimagedata.Add(new Uri(filepath));
            }
        }

        private string GetTempFolder()
        {
            var tmppath = Path.GetTempPath();

            var d = Directory.CreateDirectory(@$"{tmppath}\MediaSystemClient\GUITest");

            return d.FullName;
        }

        public Uri DownloadFileData(MediaFileInfo file, IPEndPoint iPEnd)
        {
            return _resourceimagedata[new Random().Next(0, _resourceimagedata.Count - 1)];
        }

        public Task<Uri> DownloadFileDataAsync(MediaFileInfo file, IPEndPoint iPEnd)
        {
            return Task.Run(() => _resourceimagedata[new Random().Next(0, _resourceimagedata.Count - 1)]);
        }

        public void Dispose()
        {
            foreach (var file in Directory.GetFiles(_downloadTempFolder))
            {
                File.Delete(file);
            }

            Directory.Delete(_downloadTempFolder);

            _logger.LogDebug("GUITest Temp-files deleted");
        }
    }
}