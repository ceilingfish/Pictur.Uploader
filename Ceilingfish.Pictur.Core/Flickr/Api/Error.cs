using Newtonsoft.Json;

namespace Ceilingfish.Pictur.Core.Flickr.Api
{
    public class Error
    {
        [JsonProperty("stat")]
        public string Stat { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        internal static bool TryParse(string message, out Error error)
        {
            var internalError = JsonConvert.DeserializeObject<Error>(message);

            if (internalError.Code != 0)
            {
                error = internalError;
                return true;
            }

            error = null;
            return false;
        }
    }
}
