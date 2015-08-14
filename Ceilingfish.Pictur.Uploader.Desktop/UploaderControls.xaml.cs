using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ceilingfish.Pictur.Uploader.Desktop
{
    public partial class UploaderControls : UserControl
    {
        private Core.Uploader inProcessUploader;

        public UploaderControls()
        {
            InitializeComponent();
        }

        internal void Stop()
        {
            if(inProcessUploader != null)
            {
                inProcessUploader.Stop();
            }
        }

        private async void OnToggleUploaderRunningButtonClicked(object sender, RoutedEventArgs e)
        {
            if(inProcessUploader == null)
            {
                await ToggleUploaderState("Starting", "Running", "Stop", StartInProcessUploader);
            }
            else
            {
                await ToggleUploaderState("Stopping", "Stopped", "Start", StopInProcessUploader);
            }
        }

        private async Task StartInProcessUploader()
        {
            inProcessUploader = new Core.Uploader();
            await inProcessUploader.Start();
        }

        private async Task StopInProcessUploader()
        {
            await inProcessUploader.StopAsync();
            inProcessUploader = null;
        }

        private async Task ToggleUploaderState(string intermediaryState, string targetState, string inverseState, Func<Task> action)
        {
            var intermediaryResource = Resources[$"{intermediaryState}UploaderState"] as Brush;
            var targetStateResource = Resources[$"{targetState}UploaderState"] as Brush;
            UploaderStateLabel.Content = intermediaryState;
            UploaderStateLabel.Foreground = intermediaryResource;
            ToggleUploaderRunningButton.Content = intermediaryState;
            ToggleUploaderRunningButton.IsEnabled = false;
            await action();
            UploaderStateLabel.Content = targetState;
            UploaderStateLabel.Foreground = targetStateResource;
            ToggleUploaderRunningButton.Content = inverseState;
            ToggleUploaderRunningButton.IsEnabled = true;
        }
    }
}
