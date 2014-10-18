using Ceilingfish.Pictur.Core;
using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ceilingfish.Pictur.Uploader.Desktop
{
    /// <summary>
    /// Interaction logic for ManagedDirectoryControls.xaml
    /// </summary>
    public partial class ManagedDirectoryControls
    {
        public event EventHandler<DirectoryAddedArgs> Added;

        public event EventHandler<DirectoryRemovedArgs> Removed;

        public ManagedDirectoryControls()
        {
            InitializeComponent();
        }

        private IDatabase _db;
        internal IDatabase Database
        {
            get { return _db; }
            set
            {
                _db = value;
                ManagedDirectoryGrid.Items.Clear();
                ManagedDirectoryGrid.ItemsSource = new ObservableCollection<ManagedDirectory>(_db.ManagedDirectories);
            }
        }

        private ManagedDirectoryWatcher _watcher;
        internal ManagedDirectoryWatcher Watcher
        {
            get { return _watcher; }
            set { _watcher = value; }
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
            var directory = new ManagedDirectory { Path = NewDirectoryPathTextField.Text };

            _db.Add(directory);
            _watcher.Add(directory);

            var viewItems = ManagedDirectoryGrid.ItemsSource as ObservableCollection<ManagedDirectory>;

            viewItems.Add(directory);

            Added(this, new DirectoryAddedArgs(directory));
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
                    Removed(this, new DirectoryRemovedArgs(dir));
                }
            }
        }
    }
}
