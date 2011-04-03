﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Ceilingfish.FacebookUploader.Constants
{
    public class FacebookConstants
    {
        public const string ApplicationClientId = "113238968755945";

        public const string GraphApiBaseEndpoint = "http://graph.facebook.com";

        public const string AuthenticationLandingPage = "http://www.facebook.com/connect/login_success.html";

        public static string[] AuthenticationPages = new string[]
        {
            "/dialog/oauth",
            "/login.php",
            "/connect/uiserver.php",
            "/connect/login_success.html"
        };
    }
}
