using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Events
{
    public class FileDetectedArgs
    {
        public readonly ManagedDirectory Directory;
        public string FilePath;

        public FileDetectedArgs(ManagedDirectory directory, string path)
        {
            Directory = directory;
            FilePath = path;
        }
    }
}
