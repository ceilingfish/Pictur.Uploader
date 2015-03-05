using System;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Uploader.Desktop
{
    /// <summary>
    /// Interaction logic for FileStatus.xaml
    /// </summary>
    public partial class FileStatus
    {
        internal IDatabase Database { get; set; }

        public FileStatus()
        {
            InitializeComponent();
        }

        internal void RefreshRecent()
        {
            var recentlyModified = Database.Files.RecentlyModified.ToArray();

            Recent.ItemsSource = recentlyModified.Select(f => new DbFileStatus(f));
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

        internal void PollForChanges(CancellationToken cancellationToken)
        {
            RefreshRecent();
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            timer.Tick += (sender, args) =>
            {
                RefreshRecent();
            };
            timer.Start();

            cancellationToken.Register(timer.Stop);
        }

        private void ClickResetButton(object sender, System.Windows.RoutedEventArgs e)
        {
            Database.Files.Clear();
            RefreshRecent();
        }
    }
}
