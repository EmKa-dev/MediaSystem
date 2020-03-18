using System;
using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Models;

namespace MediaSystem.DesktopClientWPF.GUITest
{
    public class GUITestDetectionService : IServerScanner
    {
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Return a fake device
        /// </summary>
        public void ScanForDevice()
        {
            if (Enabled)
            {   
                DeviceDetected?.Invoke(new GUITestDeviceMocker().MockDevice(DataMediaType.Image));

                this.Enabled = false;
            }
        }

        public event Action<DeviceInfo> DeviceDetected;
    }
}
