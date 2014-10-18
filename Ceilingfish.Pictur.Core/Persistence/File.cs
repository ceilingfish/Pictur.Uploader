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
        public DateTime ModifiedAt { get; set; }
    }
}
