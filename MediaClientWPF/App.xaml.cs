#define GUITest

using MediaSystem.DesktopClientWPF;
using MediaSystem.DesktopClientWPF.Dev.GUITest;
using MediaSystem.DesktopClientWPF.Models;
using MediaSystem.DesktopClientWPF.ViewModels;
using MediaSystem.DesktopClientWPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DesktopClientWPF
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //Add singletons
            services.AddSingleton<MainView>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<DeviceBrowserViewModel>();


            //Add child viewmodels
            services.AddTransient<ImageBrowserViewModel>();

            //Misc services
#if GUITest
            SessionLogger.LogEvent("GUITest");
            services.AddTransient<IDownloadService, GUITestDownloadService>();
            services.AddTransient<IServerScanner, GUITestDetectionService>();
#else
                        services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IServerScanner, DetectionService>();
#endif

        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainView>();
            mainWindow.Show();
        }
    }
}
