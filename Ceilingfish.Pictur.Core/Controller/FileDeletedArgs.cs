using System;
using Ceilingfish.Pictur.Core.Model;

namespace Ceilingfish.Pictur.Core.Controller
{
    public class FileDeletedArgs : EventArgs
    {
        internal readonly File File;

        internal FileDeletedArgs(File f)
        {
            File = f;
        }
    }
}
