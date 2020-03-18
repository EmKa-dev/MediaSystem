using System;
using System.Collections.Generic;

namespace MediaSystem.Communications
{
    [Serializable]
    public class DeviceInfo
    {
        public ConnectionInfo ConnectionInfo { get; set; }

        public DataMediaType MediaType { get; set; }

        public CoverInformation CoverInfo { get; set; }

        public List<MediaFileInfo> MediaFiles { get; set; } = new List<MediaFileInfo>();
    }
}
