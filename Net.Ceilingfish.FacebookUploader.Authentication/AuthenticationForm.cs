using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Net.Ceilingfish.FacebookUploader.Core;
using Net.Ceilingfish.FacebookUploader.Constants;
using System.Collections.Specialized;

namespace Net.Ceilingfish.FacebookUploader.Authentication
{
    public partial class AuthenticationForm : Form
    {
        private string AccessToken;
        private List<Uri> ValidUris = new List<Uri>();

        public AuthenticationForm()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (Browser.Url.ToString().StartsWith(FacebookConstants.AuthenticationLandingPage))
            {
                NameValueCollection queryParams = HttpUtility.ParseQueryString(Browser.Url.Fragment.Substring(1));
            }
        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            Browser.Url = new Uri(String.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=token&scope=offline_access,publish_stream", FacebookConstants.ApplicationClientId, FacebookConstants.AuthenticationLandingPage));
        }

        private void SendTestUpload()
        {
            Upload testUpload = new Upload(@"C:\Users\tomwilliams\Pictures\bert.png",AccessToken);

            testUpload.Send();
        }

        private void RedirectToBrowser(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!FacebookConstants.AuthenticationPages.Contains(e.Url.AbsolutePath))
            {
                System.Diagnostics.Process.Start(e.Url.ToString());
                e.Cancel = true;
            }
        }
    }
}
