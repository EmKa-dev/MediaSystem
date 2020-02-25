using MediaSystem.DesktopClientWPF.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MediaSystem.DesktopClientWPF.Views;
using System.Net;
using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Models;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class DeviceBrowserViewModel : BaseViewModel
    {
        #region Binding Properties

        public ObservableCollection<Device> AvailableDevices { get; set; } = new ObservableCollection<Device>();

        private Device _SelectedDevice;
        public Device SelectedDevice
        {
            get { return _SelectedDevice; }
            set
            {
                _SelectedDevice = value;
                OnPropertyChanged(this, (nameof(SelectedDevice)));
                SelectedFile = null;
            }
        }

        private MediaFileInfo _SelectedFile;
        public MediaFileInfo SelectedFile
        {
            get { return _SelectedFile; }
            set
            {
                _SelectedFile = value;
                OnPropertyChanged(this, (nameof(SelectedFile)));
            }
        }

        #endregion

        private IServerScanner _serverScanner;

        #region Commands

        public ICommand SelectDeviceCommand { get; set; }
        public ICommand DeselectDeviceCommand { get; set; }
        public ICommand SelectFileCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }

        #endregion

        public DeviceBrowserViewModel(IServerScanner serverScanner)
        {
            //Set up and start the scanner service
            _serverScanner = serverScanner;
            _serverScanner.DeviceDetected += this.OnDeviceDetection;

            StartServerDetection();

            //Create commands
            SelectDeviceCommand = new RelayCommand((device) => this.SelectedDevice = (Device)device);
            DeselectDeviceCommand = new RelayCommand((obj) => SelectedDevice = null);
            SelectFileCommand = new RelayCommand((file) => SelectedFile = (MediaFileInfo)file);
            OpenFileCommand = new RelayCommand(() => this.OpenFileView());
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

        private void OnDeviceDetection(Device device)
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

        private void OpenFileView()
        {
            //SelectedDevice informs what type of view/window need to be opened
            if (SelectedDevice == null)
            {
                return;
            }

            //Get the ipendpoint of device if it exists
            IPEndPoint iPEndPoint = null;

            if (SelectedDevice.ConnectionInfo != null)
            {
                IPAddress.TryParse(SelectedDevice.ConnectionInfo.IPAddress, out IPAddress adr);

                iPEndPoint = new IPEndPoint(adr, SelectedDevice.ConnectionInfo.Port);
            }

            switch (SelectedDevice.MediaType)
            {
                case DataMediaType.Image:

                    var imageViewer = new ImageViewer(SelectedFile, iPEndPoint);

                    imageViewer.Show();
                    break;

                case DataMediaType.Audio:

                    var musicviewer = new MusicViewer(SelectedDevice.MediaFiles, iPEndPoint);

                    musicviewer.Show();
                    break;

                case DataMediaType.Video:

                    VideoViewer videoViewer = new VideoViewer(SelectedFile, iPEndPoint);

                    videoViewer.Show();
                    break;

                default:
                    break;
            }
        }
    }
}
