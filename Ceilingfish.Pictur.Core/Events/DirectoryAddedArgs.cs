using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Events
{
    public class DirectoryAddedArgs : EventArgs
    {
        public readonly ManagedDirectory Directory;

        public DirectoryAddedArgs(ManagedDirectory d)
        {
            Directory = d;
        }
    }
}
