using MediaSystem.Communications;
using System;

//TODO: Mock device including content somehow (programmatically create white images etc.)
namespace MediaSystem.DesktopClientWPF.Dev.GUITest
{
    public class GUITestDeviceMocker
    {
        public DeviceInfo MockDevice(DataMediaType mediatype)
        {
            DeviceInfo device = new DeviceInfo
            {
                CoverInfo = new CoverInformation($"{mediatype}Type", 5),
                MediaType = mediatype,
                ConnectionInfo = new ConnectionInfo { IPAddress = "0.0.0.0", Port = 0},
            };

            var filename = "";

            switch (mediatype)
            {
                case DataMediaType.Image:
                    filename = "ImageFile.jpg";
                    break;
                case DataMediaType.Audio:
                    filename = "Audio.mp3";
                    break;
                case DataMediaType.Video:
                    filename = "VideoFile.avi";
                    break;
                default:
                    break;
            }

            for (int i = 0; i < 5; i++)
            {
                device.MediaFiles.Add(
                    new MediaFileInfo
                    {
                        FileName = $"{filename}",
                        FileMetaData = new MetaData { Author = "Author", Title = "FileTitle", RunTime = 42 } 
                    });
            }

            return device;
        }
    }
}
