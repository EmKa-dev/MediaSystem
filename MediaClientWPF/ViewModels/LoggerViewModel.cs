using System.Collections.ObjectModel;
using System.Windows;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class LoggerViewModel : BaseViewModel
    {
        public ObservableCollection<string> SessionLog { get; set; } = new ObservableCollection<string>();

        public LoggerViewModel()
        {
            SessionLogger.LoggerEvent += ((msg) => Application.Current.Dispatcher.InvokeAsync(() => this.SessionLog.Add(msg)));
        }
    }
}
