using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Events
{
    public class DirectoryRemovedArgs
    {
        public readonly ManagedDirectory Directory;

        public DirectoryRemovedArgs(ManagedDirectory d)
        {
            Directory = d;
        }
    }
}
