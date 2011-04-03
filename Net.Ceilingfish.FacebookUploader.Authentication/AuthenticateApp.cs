using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace Net.Ceilingfish.FacebookUploader.Authentication
{
    [RunInstaller(true)]
    public partial class AuthenticateApp : System.Configuration.Install.Installer
    {
        public AuthenticateApp()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);
            AuthenticationForm form = new AuthenticationForm();
            form.ShowDialog();
        }
    }
}
