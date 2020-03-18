using MediaSystem.Communications;

//TODO: Mock device including content somehow (programmatically create white images etc.)
namespace MediaSystem.DesktopClientWPF.GUITest
{
    public class GUITestDeviceMocker
    {
        public DeviceInfo MockDevice(DataMediaType mediatype)
        {
            DeviceInfo device = new DeviceInfo
            {
                CoverInfo = new CoverInformation("MockType", 5),
                MediaType = mediatype,
                ConnectionInfo = new ConnectionInfo { IPAddress = "0.0.0.0", Port = 0},
            };

            for (int i = 0; i < 5; i++)
            {
                device.MediaFiles.Add(new MediaFileInfo { FileName = $"Mock{i}", FileMetaData = new MetaData { Author = "Author", Title = "FileTitle", RunTime = 42 } });
            }

            return device;
        }
    }
}
