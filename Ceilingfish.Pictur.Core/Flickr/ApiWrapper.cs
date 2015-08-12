using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Ceilingfish.Pictur.Core.Flickr.Api;
using Ceilingfish.Pictur.Core.Persistence;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class ApiWrapper
    {
        private const string ApiUrl = @"https://api.flickr.com/services/rest/";
        private const string ReplaceUrl = @"https://up.flickr.com/services/replace/";
        private const string UploadUrl = @"https://up.flickr.com/services/upload/";

        private readonly IDatabase _db;

        private string ApiKey
        {
            get { return _db.Settings.Flickr.ApiKey; }
        }

        private string ApiSecret
        {
            get { return _db.Settings.Flickr.ApiSecret; }
        }

        private string OAuthToken
        {
            get { return _db.Settings.Flickr.OAuthToken; }
        }

        private string UserId
        {
            get { return _db.Settings.Flickr.UserId; }
        }

        public ApiWrapper(IDatabase db)
        {
            _db = db;
        }

        public IEnumerable<PhotoSearchInfo> SearchByName(string name)
        {
            var photos = CallApi<PhotoSearchResult>("flickr.photos.search", new Dictionary<string, string>
			{

				{"text", "IMG_2097"},
				{"user_id", UserId},
                {"extras","url_o,date_taken"}
			});

            return photos.Results;
        }

        public PhotoGetInfoResult GetPhotoInfo(string photoId, string secret = null)
        {
            var parameters = new Dictionary<string, string>
			{
				{ "photo_id", photoId }
			};

            if (!string.IsNullOrEmpty(secret))
                parameters["secret"] = secret;

            try
            {
                return CallApi<PhotoGetInfoResult>("flickr.photos.getInfo", parameters);
            }
            catch (ApiException e)
            {
                if (e.Error != null && e.Error.Code == 1)
                    return null;
                throw;
            }
        }

        private T CallApi<T>(string method, Dictionary<string, string> parameters)
        {
            parameters = new Dictionary<string, string>(parameters)
			{
				{"method", method},
				{"api_key", ApiKey},
				{"format", "json"},
				{"nojsoncallback", "1"},
				{"oauth_version", "1.0"},
				{"oauth_consumer_key", ApiKey},
				{"oauth_token", _db.Settings.Flickr.OAuthToken}
			};
            var queryString = HttpUtility.ParseQueryString("");
            foreach (var entry in parameters)
            {
                queryString[entry.Key] = entry.Value;
            }
            var uri = new UriBuilder(ApiUrl) { Query = queryString.ToString() };

            var client = new WebClient { Encoding = Encoding.UTF8 };
            try
            {
                var response = client.DownloadString(uri.ToString());
                Error error;
                if (Error.TryParse(response, out error))
                {
                    throw new ApiException(error);
                }
                var packet = JsonConvert.DeserializeObject<T>(response);


                return packet;
            }
            catch (WebException e)
            {
                using (var response = e.Response)
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(responseStream))
                        {
                            throw new ApiException(streamReader.ReadToEnd(), ((HttpWebResponse)response).StatusCode);
                        }
                    }
                }
            }
        }

        internal string UploadPhoto(string file, string name)
        {
            var client = new FlickrNet.Flickr(ApiKey);
            return client.UploadPicture(file, name);
        }
    }
}
