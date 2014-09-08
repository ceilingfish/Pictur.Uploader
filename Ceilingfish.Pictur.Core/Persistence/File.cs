using System;
using System.IO;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class File
    {
        private FileInfo _file;

        public File()
        {
            
        }

        public File(FileInfo fileInfo)
        {
            _file = fileInfo;
            LastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            FileLength = fileInfo.Length;
        }

        public DateTime LastWriteTimeUtc { get; set; }

        public string Checksum { get; set; }

        public long FileLength { get; set; }

        public string Path
        {
            get { return _file.FullName; }
            set { _file = new FileInfo(value); }
        }
    }
}
