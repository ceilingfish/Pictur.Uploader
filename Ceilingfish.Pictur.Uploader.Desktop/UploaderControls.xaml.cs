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

        private void OnToggleUploaderRunningButtonClicked(object sender, RoutedEventArgs e)
        {
            if(inProcessUploader == null)
            {
                UploaderStateLabel.Content = "Starting";
                UploaderStateLabel.Foreground = Brushes.LightGoldenrodYellow;
                ToggleUploaderRunningButton.Content = "Starting";
                ToggleUploaderRunningButton.IsEnabled = false;
                inProcessUploader = new Core.Uploader();
                inProcessUploader.Start();
                UploaderStateLabel.Content = "Running";
                UploaderStateLabel.Foreground = Brushes.Green;
                ToggleUploaderRunningButton.Content = "Stop";
                ToggleUploaderRunningButton.IsEnabled = true;
            }
            else
            {
                UploaderStateLabel.Content = "Stopping";
                UploaderStateLabel.Foreground = Brushes.LightGoldenrodYellow;
                ToggleUploaderRunningButton.Content = "Stopping";
                ToggleUploaderRunningButton.IsEnabled = false;
                inProcessUploader.Stop();
                inProcessUploader = null;
                UploaderStateLabel.Content = "Stopped";
                UploaderStateLabel.Foreground = Brushes.Red;
                ToggleUploaderRunningButton.Content = "Start";
                ToggleUploaderRunningButton.IsEnabled = true;
            }
        }
    }
}
