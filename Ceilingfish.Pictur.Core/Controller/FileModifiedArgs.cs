using System;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Controller
{
    public class FileModifiedArgs : EventArgs
    {
        internal readonly File Original;
        internal readonly File New;

        internal FileModifiedArgs(File o, File n)
        {
            Original = o;
            New = n;
        }
    }
}