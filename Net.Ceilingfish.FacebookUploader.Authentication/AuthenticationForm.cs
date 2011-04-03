using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Net.Ceilingfish.FacebookUploader.Core;
using Net.Ceilingfish.FacebookUploader.Constants;

namespace Net.Ceilingfish.FacebookUploader.Authentication
{
    public partial class AuthenticationForm : Form
    {
        private string AccessToken;

        public AuthenticationForm()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (Browser.Url.ToString().StartsWith(FacebookConstants.AuthenticationLandingPage))
            {
                System.Web.HttpUtility.ParseQueryString();
                string queryString = Browser.Url.ToString().Replace(FacebookConstants.AuthenticationLandingPage,"");
                MessageBox.Show(String.Format("Authenticated with query string:{0}", queryString));
            }
        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            Browser.Url = new Uri(String.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=token", FacebookConstants.ApplicationClientId, FacebookConstants.AuthenticationLandingPage));
        }

        private void SendTestUpload()
        {
            Upload testUpload = new Upload(@"C:\Users\tomwilliams\Pictures\bert.png",AccessToken);

            testUpload.Send();
        }
    }
}
