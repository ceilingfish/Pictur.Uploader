using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using Ceilingfish.Pictur.Core;
using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Models;
using MessageBox = System.Windows.MessageBox;
using Ceilingfish.Pictur.Core.Persistence;
using Directory = System.IO.Directory;

namespace Ceilingfish.Pictur.Uploader.Desktop
{
    /// <summary>
    /// Interaction logic for ManagedDirectoryControls.xaml
    /// </summary>
    public partial class ManagedDirectoryControls
    {
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
            }
        }

        internal void RefreshDirectories()
        {
            ManagedDirectoryGrid.Items.Clear();
            ManagedDirectoryGrid.ItemsSource = new ObservableCollection<Core.Models.Directory>(_db.Directories);
        }

        private void OnBrowseForDirectoryClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
                NewDirectoryPathTextField.Text = dialog.SelectedPath;
        }

        private void OnAddDirectoryClicked(object sender, RoutedEventArgs e)
        {
            var directory = new Core.Models.Directory { Path = Path.GetFullPath(NewDirectoryPathTextField.Text) };

            _db.Directories.Add(directory);

            var viewItems = ManagedDirectoryGrid.ItemsSource as ObservableCollection<Core.Models.Directory>;

            viewItems.Add(directory);

            NewDirectoryPathTextField.Text = "";
        }

        public bool IsValidNewDirectory
        {
            get
            {
                var path = NewDirectoryPathTextField.Text;
                if (string.IsNullOrEmpty(path))
                    return false;
                path = Path.GetFullPath(path);

                return Directory.Exists(path)
                    && !_db.Directories.Any(d => d.Path.Equals(path));
            }
        }

        private void OnNewDirectoryPathChanged(object sender, TextChangedEventArgs e)
        {
            AddDirectoryButton.GetBindingExpression(IsEnabledProperty).UpdateTarget();
        }

        private void OnEditDirectoryClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnRemoveDirectoryClicked(object sender, RoutedEventArgs e)
        {
            var hyperlink = e.OriginalSource as Hyperlink;

            var decision = MessageBox.Show("Are you sure you wish to remove this directory? Uploaded files will not be removed", "Confirm Directory Remove", MessageBoxButton.YesNo);

            if (decision == MessageBoxResult.Yes)
            {
                var sourceDir = hyperlink.DataContext as Core.Models.Directory;
                var dir = _db.Directories.Single(d => d.Id.Equals(sourceDir.Id));

                _db.Directories.Remove(dir);

                var viewItems = ManagedDirectoryGrid.ItemsSource as ObservableCollection<Core.Models.Directory>;
                var removableViewItem = viewItems.Single(d => d.Id.Equals(dir.Id));
                viewItems.Remove(removableViewItem);
            }
        }
    }
}
