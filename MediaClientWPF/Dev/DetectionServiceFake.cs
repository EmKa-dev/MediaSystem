using System;
using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Models;

namespace MediaSystem.DesktopClientWPF.Dev
{
    public class DetectionServiceFake : IServerScanner
    {
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Return a fake device
        /// </summary>
        public void ScanForDevice()
        {
            if (Enabled)
            {   
                DeviceDetected?.Invoke(Mocker.MockDevice());

                this.Enabled = false;
            }
        }

        public event Action<DeviceInfo> DeviceDetected;
    }
}
