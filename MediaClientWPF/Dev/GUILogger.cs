using DesktopClientWPF;
using MediaSystem.DesktopClientWPF.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MediaSystem.DesktopClientWPF.Dev
{
	internal class GUILogger : ILogger
    {
        public event Action<string> LogMessageEvent;

		public GUILogger()
		{
			SetupGUILogger();
		}

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogMessageEvent?.Invoke(state.ToString());
        }

		private void SetupGUILogger()
		{
			var mview = App.ServiceProvider.GetService<MainView>();
			Grid grid = mview.MainContentGrid;

			//Add a row at bottom
			var g = new RowDefinition
			{
				Height = new GridLength(140)
			};
			grid.RowDefinitions.Add(g);


			//Add debug log window
			ListView list = new ListView();
			list.SetValue(Grid.RowProperty, grid.RowDefinitions.Count - 1);
			list.BorderThickness = new Thickness(5);
			list.BorderBrush = new SolidColorBrush(Colors.CadetBlue);
			list.Margin = new Thickness(15);

			grid.Children.Add(list);

			this.LogMessageEvent += l;

			void l(string s)
			{
				App.Current.Dispatcher.Invoke(() =>
				{
					list.Items.Add(s);
					list.Items.Refresh();
					list.ScrollIntoView(list.Items[list.Items.Count - 1]);
				});
			}
		}
	}
}
