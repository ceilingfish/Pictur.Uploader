using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ceilingfish.Pictur.Core;

namespace Ceilingfish.FacebookUploader.Ui
{
    public partial class MainWindow
    {
        public ManagedDirectorySet ManagedDirectorySet { get; set; }

        public MainWindow()
        {
            ManagedDirectorySet = new ManagedDirectorySet();
            InitializeComponent();
        }

        private void OnAddManagedDirectory(object sender, RoutedEventArgs e)
        {
            ManagedDirectorySet.Add(NewManagedDirectoryTextbox.Text);
        }
    }
}
