using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.DIServices;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
	class MainViewModel : BaseViewModel
    {
		DeviceBrowserViewModel _deviceBrowserViewModel;

		public LoggerViewModel LoggerViewModel { get; set; }

		private BaseViewModel _currentViewModel;

		public BaseViewModel CurrentViewModel
		{
			get { return _currentViewModel; }
			set
			{
				_currentViewModel = value;
				OnPropertyChanged(this, (nameof(CurrentViewModel)));
			}
		}

		public MainViewModel()
		{

			_deviceBrowserViewModel = IoTContainer.GetDeviceBrowserVM();
			_deviceBrowserViewModel.DeviceDetectedEvent += OnDeviceChanged;

			CurrentViewModel = _deviceBrowserViewModel;

			LoggerViewModel = new LoggerViewModel();
		}

		private void OnDeviceChanged(DeviceInfo deviceInfo)
		{
			switch (deviceInfo.MediaType)
			{
				case DataMediaType.Image:
					CurrentViewModel = new ImageBrowserViewModel(deviceInfo);
					break;
				case DataMediaType.Audio:
					break;
				case DataMediaType.Video:
					break;
				default:
					break;
			}
		}
	}
}
