using Ceilingfish.Pictur.Core.Models;
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
using System.Windows.Threading;
using Ceilingfish.Pictur.Core.Pipeline;

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
            Recent.ItemsSource = Database
                                .Files
                                .RecentlyModified
                                .Select(f => new DbFileStatus(f));
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
    }
}
