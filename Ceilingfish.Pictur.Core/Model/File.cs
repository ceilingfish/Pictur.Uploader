using System;
using System.IO;

namespace Ceilingfish.Pictur.Core.Model
{
    public class File
    {
        public File(FileInfo fileInfo)
        {
            LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            Path = fileInfo.FullName;
            FileLength = fileInfo.Length;
        }

        public DateTime LastWriteTimeUtc { get; set; }

        public string Path { get; set; }

        public string Checksum { get; set; }

        public long FileLength { get; set; }
    }
}
