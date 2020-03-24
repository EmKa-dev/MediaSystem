using DesktopClientWPF;
using MediaSystem.Communications;
using MediaSystem.DesktopClientWPF.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows.Input;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
	class MainViewModel : BaseViewModel
    {
		IServiceScope _serviceScope;

		private ILogger _logger;

		public ICommand BackToDevicesCommand { get; set; }

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
			_serviceScope = App.ServiceProvider.CreateScope();

			_logger = logger;

			BackToDevicesCommand = new RelayCommand(() => 
			{
				SetCurrentViewModelToBrowser();
				DisposeAndCreateNewScope();

			}, new System.Func<object, bool>((o) => !(CurrentViewModel is DeviceBrowserViewModel)));

			var deviceBrowserViewModel = App.ServiceProvider.GetService<DeviceBrowserViewModel>();
			deviceBrowserViewModel.DeviceDetectedEvent += OnDeviceChanged;
			CurrentViewModel = deviceBrowserViewModel;

			deviceBrowserViewModel.StartServerDetection();
		}

		private bool IsCurrentViewDeviceBrowser()
		{
			return !(CurrentViewModel is DeviceBrowserViewModel);
		}

		private void SetCurrentViewModelToBrowser()
		{
			CurrentViewModel = App.ServiceProvider.GetService<DeviceBrowserViewModel>();
		}

		private void DisposeAndCreateNewScope()
		{
			_serviceScope.Dispose();

			_serviceScope = App.ServiceProvider.CreateScope();
		}

		private void OnDeviceChanged(DeviceInfo deviceInfo)
		{


			switch (deviceInfo.MediaType)
			{
				case DataMediaType.Image:

					var imgvm = _serviceScope.ServiceProvider.GetService<ImageBrowserViewModel>();
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
