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
using System.Linq;

namespace MediaSystem.DesktopClientWPF.Dev.GUITest
{
    public class GUITestDownloadService : IDownloadService, IDisposable
    {
        private ILogger _logger;

        private readonly string _downloadTempFolder;

        public GUITestDownloadService(ILogger logger)
        {
            _logger = logger;
            _downloadTempFolder = GetTempFolder();
        }

        private List<Uri> CreateTestImages()
        {
            var images = TestImageResources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            List<Uri> resourceimagedata = new List<Uri>();

            foreach (DictionaryEntry item in images)
            {

                byte[] b = item.Value as byte[];

                string filepath = $"{_downloadTempFolder}/{item.Key.ToString()}.png";
                var fs = File.Create(filepath);
                fs.Write(b);
                fs.Close();

                resourceimagedata.Add(new Uri(filepath));
            }

            return resourceimagedata;
        }

        private List<Uri> CreateTestVideos()
        {
            var videos = TestVideoResources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            List<Uri> resourcevideodata = new List<Uri>();

            foreach (DictionaryEntry item in videos)
            {

                byte[] b = item.Value as byte[];
                string filepath = $"{_downloadTempFolder}/{item.Key.ToString()}.mp4";
                var fs = File.Create(filepath);
                fs.Write(b);
                fs.Close();

                resourcevideodata.Add(new Uri(filepath));
            }

            return resourcevideodata;
        }

        private string GetTempFolder()
        {
            var tmppath = Path.GetTempPath();

            var d = Directory.CreateDirectory(@$"{tmppath}\MediaSystemClient\GUITest");

            return d.FullName;
        }

        public Uri DownloadFileData(MediaFileInfo file, IPEndPoint iPEnd)
        {

            List<Uri> TestResources = new List<Uri>();

            switch (GetMediaType(file.FileName))
            {
                case DataMediaType.Image:
                    TestResources.AddRange(CreateTestImages());
                    break;
                case DataMediaType.Audio:
                    break;
                case DataMediaType.Video:
                    TestResources.AddRange(CreateTestVideos());
                    break;
                default:
                    break;
            }

            return TestResources[new Random().Next(0, TestResources.Count - 1)];
        }

        public Task<Uri> DownloadFileDataAsync(MediaFileInfo file, IPEndPoint iPEnd)
        {
            return Task.Run(() => CreateTestImages()[0]);
        }

        private DataMediaType GetMediaType(string filename)
        {
            //Collection of possible extension to check files against
            string[] imagetypes = { ".jpg", ".jpeg", ".png", ".bmp" };
            string[] videotypes = { ".mp4", ".avi" };
            string[] musictypes = { ".mp3", ".wav" };

           var fileext = Path.GetExtension(filename);

            //Check our extensions against possible extensions
            //to determine what type of file
            if (imagetypes.Any((s) => { return fileext == s; }))
            {
                return DataMediaType.Image;
            }
            else if (videotypes.Any((s) => { return fileext == s; }))
            {
                return DataMediaType.Video;
            }
            else if (musictypes.Any((s) => { return fileext == s; }))
            {
                return DataMediaType.Audio;
            }

            return 0;
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