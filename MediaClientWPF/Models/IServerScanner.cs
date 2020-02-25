using System;

namespace MediaSystem.DesktopClientWPF.Models
{
    public interface IServerScanner
    {
        /// <summary>
        /// If the scanner is enabled, if false, method <see cref="ScanForDevice"/> does nothing.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Scan the network for available device once
        /// </summary>
        void ScanForDevice();

        /// <summary>
        /// Event that is fire when a device is detected
        /// </summary>
        event Action<Communications.Device> DeviceDetected;
    }
}
