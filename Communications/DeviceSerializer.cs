using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MediaSystem.Communications
{
    public class DeviceSerializer
    {
        public byte[] Serialize(DeviceInfo device)
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, device);
                return stream.ToArray();
            }
        }

        public DeviceInfo Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return (DeviceInfo)new BinaryFormatter().Deserialize(stream);
            }
        }
    }
}
