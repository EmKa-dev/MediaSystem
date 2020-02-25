using MediaSystem.DesktopClientWPF.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MediaSystem.DesktopClientWPF.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(object sender, string property)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(property));
        }

        public ICommand TestCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public BaseViewModel()
        {
            CloseCommand = new RelayCommand(() => this.CloseWindow());

            TestCommand = new RelayCommand((obj) => MessageBox.Show(obj.ToString()));

        }

        protected virtual void CloseWindow()
        {

            foreach (UIElement window in Application.Current.Windows)
            {
                if (window.IsMouseOver)
                {

                    //If window is the main window, shutdown app
                    if (window == Application.Current.MainWindow)
                    {
                        Application.Current.Shutdown();
                    }

                    var w = window as Window;

                    w.Close();
                    return;
                }
            }

            MessageBox.Show("Closing window failed");
        }
    }
}
