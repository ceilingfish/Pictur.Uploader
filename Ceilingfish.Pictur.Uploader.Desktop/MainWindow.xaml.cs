using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Ceilingfish.Pictur.Core.FileSystem;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Uploader.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly CancellationTokenSource _cancellationToken;

        public MainWindow()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Ceilingfish.Uploadr", "Persistence.Raven");
            var db = new RavenDatabase(path);
            _cancellationToken = new CancellationTokenSource();

            InitializeComponent();
            DirectoryControls.Database = db;
            FileStatus.Database = db;
            DirectoryControls.RefreshDirectories();
            FileStatus.PollForChanges(_cancellationToken.Token);

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _cancellationToken.Cancel();
            base.OnClosing(e);
        }
    }
}