using DesktopClientWPF;
using MediaSystem.Communications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
	class MainViewModel : BaseViewModel
    {
		private ILogger _logger;

		DeviceBrowserViewModel _deviceBrowserViewModel;

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

		public MainViewModel(ILogger logger)
		{
			_logger = logger;

			_deviceBrowserViewModel = App.ServiceProvider.GetService<DeviceBrowserViewModel>();
			_deviceBrowserViewModel.DeviceDetectedEvent += OnDeviceChanged;
			CurrentViewModel = _deviceBrowserViewModel;

			_deviceBrowserViewModel.StartServerDetection();
		}

		private void OnDeviceChanged(DeviceInfo deviceInfo)
		{

			switch (deviceInfo.MediaType)
			{
				case DataMediaType.Image:

					var imgvm = App.ServiceProvider.GetService<ImageBrowserViewModel>();
					imgvm.InitializeDeviceData(deviceInfo);
					CurrentViewModel = imgvm;

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
