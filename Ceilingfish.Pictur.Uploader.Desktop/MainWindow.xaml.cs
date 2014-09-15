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

namespace Ceilingfish.Pictur.Uploader.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDatabase _db;
        private readonly ManagedDirectoryEventSource _source;

        public MainWindow()
        {
            InitializeComponent();

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Ceilingfish.Uploadr", "Persistence.Raven");
            _db = new RavenDatabase(path);

            _source = new ManagedDirectoryEventSource(_db.ManagedDirectories);
            var consoleUploader = new ConsoleUploader(_db);
            _source.Added += consoleUploader.OnFileAdded;
            _source.Removed += consoleUploader.OnFileRemoved;

            ManagedDirectoryGrid.Items.Clear();
            ManagedDirectoryGrid.ItemsSource = new ObservableCollection<ManagedDirectory>(_db.ManagedDirectories);
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
            _source.Add(directory);

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
    }
}
