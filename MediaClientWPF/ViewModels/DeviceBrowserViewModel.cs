using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Commands;
using MediaSystem.DesktopClientWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class DeviceBrowserViewModel : BaseViewModel
    {

        public ObservableCollection<DeviceInfo> AvailableDevices { get; set; } = new ObservableCollection<DeviceInfo>();

        private IServerScanner _serverScanner;

        public event Action<DeviceInfo> DeviceDetectedEvent;

        public ICommand SelectDeviceCommand { get; set; }

        public DeviceBrowserViewModel(IServerScanner serverScanner)
        {
            //Set up and start the scanner service
            _serverScanner = serverScanner;
            _serverScanner.DeviceDetected += this.OnDeviceDetection;

            SelectDeviceCommand = new RelayCommand((device) => this.DeviceDetectedEvent.Invoke((DeviceInfo)device));

            StartServerDetection();
        }

        private async void StartServerDetection()
        {
            SessionLogger.LogEvent("Started searching for device");

            await Task.Run(() =>
            {
                while (_serverScanner.Enabled)
                {
                    _serverScanner.ScanForDevice();

                    Thread.Sleep(2000);
                }
            });

            SessionLogger.LogEvent("Stopped searching for device");
        }

        private void OnDeviceDetection(DeviceInfo device)
        {
            //Since functionality to not detect duplicates is not implemented/not working, currently disable detectionservice after we detected one device.
            _serverScanner.Enabled = false;

            ////TODO: This does not seem to work properly, find alternative.
            //if (AvailableDevices.ToList().Exists(x => x.Equals(device)))
            //{
            //    return;
            //}

            SessionLogger.LogEvent("Device detected");

            //Should we use synchronizationObject instead?
            Application.Current.Dispatcher.InvokeAsync(() => this.AvailableDevices.Add(device));
        }
    }
}
