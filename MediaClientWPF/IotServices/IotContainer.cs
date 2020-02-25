using MediaSystem.DesktopClientWPF.Dev;
using MediaSystem.DesktopClientWPF.Models;
using MediaSystem.DesktopClientWPF.ViewModels;

namespace MediaSystem.DesktopClientWPF.DIServices
{
    public static class IoTContainer
    {
        public static DeviceBrowserViewModel GetDeviceBrowserVM()
        {
            IServerScanner scanner = new DetectionService();

            if (scanner is DetectionServiceFake)
            {
                SessionLogger.LogEvent("A mock server detection service is used, some functionality may not work as expected");
            }
            else if(scanner is DetectionService)
            {
                SessionLogger.LogEvent("A live server detection service is used, searching for server on you local network");
            }

            return new DeviceBrowserViewModel(scanner);
        }
    }
}