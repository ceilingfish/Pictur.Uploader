using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Ceilingfish.Pictur.Core;
using Ceilingfish.Pictur.Core.Persistence;
using System.IO;
using Microsoft.Win32;
using System.Windows.Documents;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Uploader.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDatabase _db;
        private readonly ManagedDirectoryWatcher _watcher;
        private readonly ManagedDirectoryScanner _scanner;

        public MainWindow()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Ceilingfish.Uploadr", "Persistence.Raven");
            _db = new RavenDatabase(path);
            _watcher = new ManagedDirectoryWatcher(_db.ManagedDirectories);

            InitializeComponent();
            DirectoryControls.Database = _db;
            DirectoryControls.Watcher = _watcher;
            this.FileStatus.Database = _db;
            this.FileStatus.UpdateRecentFiles();

            _scanner = new ManagedDirectoryScanner(_db.ManagedDirectories);

            var databaseThinger = new DatabaseFileVerifier(_db);
            _watcher.Added += databaseThinger.OnFileAdded;
            _watcher.Removed += databaseThinger.OnFileRemoved;
            _scanner.Detected += databaseThinger.OnFileDetected;
            _scanner.ScanAsync();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _scanner.Cancel();
            _watcher.Dispose();
            this.FileStatus.StopRefresh();
            base.OnClosing(e);
        }
    }
}