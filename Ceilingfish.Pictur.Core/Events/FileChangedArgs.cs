using System;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Events
{
    public class FileChangedArgs : EventArgs
    {
        public readonly ManagedDirectory OriginalDirectory, NewDirectory;
        public readonly string OriginalPath, NewPath;
    }
}
