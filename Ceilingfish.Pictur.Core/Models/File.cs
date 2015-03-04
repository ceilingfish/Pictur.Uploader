using System;
using Ceilingfish.Pictur.Core.Models;

namespace Ceilingfish.Pictur.Core.Models
{
    public class File : BaseRecord
    {
        public string DirectoryId { get; set; }
        public string Checksum { get; set; }
        public string Path { get; set; }
        public bool Deleted { get; set; }
    }
}
