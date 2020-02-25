using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MediaSystem.Communications
{
    public class DeviceSerializer
    {
        public byte[] Serialize(Device device)
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, device);
                return stream.ToArray();
            }
        }

        public Device Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return (Device)new BinaryFormatter().Deserialize(stream);
            }
        }
    }
}
