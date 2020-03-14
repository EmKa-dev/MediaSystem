using MediaSystem.Communications;

//TODO: Mock device including content somehow (programmatically create white images etc.)
namespace MediaSystem.DesktopClientWPF.Dev
{
    public static class Mocker
    {

        public static DeviceInfo MockDevice()
        {
            DeviceInfo device = new DeviceInfo
            {
                CoverInfo = new CoverInformation("MockType", 5)
            };

            for (int i = 0; i < 5; i++)
            {
                device.MediaFiles.Add(new MediaFileInfo { FileName = $"Mock{i}", FileMetaData = new MetaData { Author = "Test", Title = "FileTitle", RunTime = 42 } });
            }

            return device;
        }
    }
}
