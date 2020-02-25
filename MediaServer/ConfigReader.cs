using System.IO;
using System.Text.Json;

namespace MediaSystem.MediaServer
{
    internal class ConfigReader
    {
        public static string GetContentFolderPathFromConfig()
        {
            using (FileStream fs = new FileStream(@".\Configurations\DeviceConfig.json", FileMode.Open, FileAccess.Read))
            {
                using (JsonDocument doc = JsonDocument.Parse(fs))
                {
                    var path = doc.RootElement.GetProperty("FolderPath").GetString();

                    return path;
                } 
            }
        }
    }
}
