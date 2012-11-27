﻿using System.ComponentModel;
using File = Ceilingfish.Pictur.Core.Model.File;

namespace Ceilingfish.Pictur.Core.Controller
{
    public class FileCreatedArgs : CancelEventArgs
    {
        internal readonly File File;

        internal FileCreatedArgs(File f)
        {
            File = f;
        }
    }
}