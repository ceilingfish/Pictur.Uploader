using System.Windows;

namespace Ceilingfish.Pictur.Facebook
{
    public partial class MainWindow
    {
        private readonly Core.App _app;

        public MainWindow()
        {
            InitializeComponent();
            _app = new Core.App();
            
            _app.Start();
        }

        private void OnAddManagedDirectory(object sender, RoutedEventArgs e)
        {
            _app.ManagedDirectories.AddDirectory(NewManagedDirectoryTextbox.Text);
        }
    }
}
