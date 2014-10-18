using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// <summary>
    /// Interaction logic for FileStatus.xaml
    /// </summary>
    public partial class FileStatus : UserControl
    {
        private readonly CancellationTokenSource _token = new CancellationTokenSource();

        internal IDatabase Database { get; set; }

        public FileStatus()
        {
            InitializeComponent();
           // Task.Run((Action)RefreshRecent, _token.Token);
        }

        internal void RefreshRecent()
        {
            while (!_token.IsCancellationRequested)
            {
                Dispatcher.Invoke(UpdateRecentFiles);
                Thread.Sleep(100);
            }
        }

        internal void UpdateRecentFiles()
        {
            if (Database == null)
                return;

            Recent.ItemsSource = Database
                                    .RecentlyModifiedFiles
                                    .Select(f => new DbFileStatus(f));
        }

        internal void StopRefresh()
        {
            _token.Cancel();
        }

        class DbFileStatus
        {
            public string Name { get; set; }
            public string Path { get; set; }

            public DbFileStatus(File file)
            {
                Path = file.Path;
                Name = System.IO.Path.GetFileName(file.Path);
            }
        }
    }
}
