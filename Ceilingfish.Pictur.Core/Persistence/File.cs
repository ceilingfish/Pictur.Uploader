using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class File
    {
        public string Id { get; set; }
        public string Checksum { get; set; }
        public string Path { get; set; }

        private List<FilePluginState> _uploads;
        public IEnumerable<FilePluginState> Uploads
        {
            get
            {
                return _uploads ?? Enumerable.Empty<FilePluginState>();
            }
            set
            {
                if (value != null)
                {
                    var values = value.ToList();
                    if (values.Count > 0)
                        _uploads = values;
                }
            }
        }

        public DateTime ModifiedAt { get; set; }
        public bool FileRemoved { get; set; }
    }
}
