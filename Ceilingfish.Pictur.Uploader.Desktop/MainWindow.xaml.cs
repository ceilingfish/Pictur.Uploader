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
            InitializeComponent();

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Ceilingfish.Uploadr", "Persistence.Raven");
            _db = new RavenDatabase(path);

            _watcher = new ManagedDirectoryWatcher(_db.ManagedDirectories);
            _scanner = new ManagedDirectoryScanner(_db.ManagedDirectories);

            var databaseThinger = new DatabaseFileVerifier(_db);
            _watcher.Added += databaseThinger.OnFileAdded;
            _watcher.Removed += databaseThinger.OnFileRemoved;
            _scanner.Detected += databaseThinger.OnFileDetected;

            ManagedDirectoryGrid.Items.Clear();
            ManagedDirectoryGrid.ItemsSource = new ObservableCollection<ManagedDirectory>(_db.ManagedDirectories);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _watcher.Dispose();
            base.OnClosing(e);
        }

        private void OnBrowseForDirectoryClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                NewDirectoryPathTextField.Text = dialog.SelectedPath;
        }

        private void OnAddDirectoryClicked(object sender, RoutedEventArgs e)
        {
            var directory = new ManagedDirectory {Path = NewDirectoryPathTextField.Text};
            
            _db.Add(directory);
            _watcher.Add(directory);

            var viewItems = ManagedDirectoryGrid.ItemsSource as ObservableCollection<ManagedDirectory>;

            viewItems.Add(directory);
        }

        public bool IsValidNewDirectory
        {
            get
            {
                var path = NewDirectoryPathTextField.Text;
                return !string.IsNullOrEmpty(path)
                    && Directory.Exists(path)
                    && !_db.ManagedDirectories.Any(d => d.Path.Equals(path));
            }
        }

        private void OnNewDirectoryPathChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            AddDirectoryButton.GetBindingExpression(IsEnabledProperty).UpdateTarget();
        }

        private void OnEditDirectoryClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnRemoveDirectoryClicked(object sender, RoutedEventArgs e)
        {
            var hyperlink = e.OriginalSource as Hyperlink;

            var decision = System.Windows.MessageBox.Show("Are you sure you wish to remove this directory? Uploaded files will not be removed", "Confirm Directory Remove", MessageBoxButton.YesNo);

            if (decision == MessageBoxResult.Yes)
            {
                var viewItems = ManagedDirectoryGrid.ItemsSource as ObservableCollection<ManagedDirectory>;
                var sourceDir = hyperlink.DataContext as ManagedDirectory;
                var matches = _db.ManagedDirectories.Where(d => d.Id.Equals(sourceDir.Id));

                foreach (var dir in matches)
                {
                    _db.Remove(dir);
                    _watcher.Remove(dir);

                    var removableViewItem = viewItems.Single(d => d.Id.Equals(dir.Id));
                    viewItems.Remove(removableViewItem);
                }
            }
        }
    }
}