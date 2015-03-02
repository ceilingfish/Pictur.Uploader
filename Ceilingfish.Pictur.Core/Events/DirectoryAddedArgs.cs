using System;
using Ceilingfish.Pictur.Core.Models;

namespace Ceilingfish.Pictur.Core.Events
{
    public class DirectoryAddedArgs : EventArgs
    {
        public readonly Directory Directory;

        public DirectoryAddedArgs(Directory d)
        {
            Directory = d;
        }
    }
}
