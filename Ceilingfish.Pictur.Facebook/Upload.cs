using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Net.Ceilingfish.FacebookUploader.Core
{
    public class Upload
    {
        public const string UploadBaseEndpoint = "http://graph.facebook.com";

        public readonly string AccessToken;

        public readonly string File;

        public Upload(string path, string token)
        {
            FileInfo inf = new FileInfo(path);

            if (!inf.Exists)
                throw new InvalidOperationException("Cannot upload a file that does not exist");

            File = path;
            AccessToken = token;
        }

        public void Send()
        {
            WebClient client = new WebClient();

            client.UploadFileAsync(CreateUri(), File);
        }

        public Uri CreateUri()
        {
            return new Uri(String.Format("{0}/{1}?access_token={2}", UploadBaseEndpoint, "me/photos", AccessToken));
        }
    }
}
