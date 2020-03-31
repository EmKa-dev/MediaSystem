using MediaSystem.DesktopClientWPF.Dev;
using MediaSystem.DesktopClientWPF.Dev.GUITest;
using MediaSystem.DesktopClientWPF.Models;
using MediaSystem.DesktopClientWPF.ViewModels;
using MediaSystem.DesktopClientWPF.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
            services.AddTransient<VideoBrowserViewModel>();

            //Misc services

#if GUITEST
            services.AddSingleton<ILogger, GUILogger>();
            services.AddTransient<IDownloadService, GUITestDownloadService>();
            services.AddTransient<IServerScanner, GUITestDetectionService>();

#elif DEBUG
            services.AddTransient<IDownloadService, DownloadService>();
            services.AddTransient<IServerScanner, DetectionService>();
            services.AddSingleton<ILogger, GUILogger>();
#endif

        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainView>();
            mainWindow.DataContext = ServiceProvider.GetService<MainViewModel>();
            mainWindow.Show();
        }
    }
}
