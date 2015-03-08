using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class Settings
    {
        public string ApiKey { get; set; }
        public bool IsActive { get { return !string.IsNullOrEmpty(ApiKey); } }
    }
}
