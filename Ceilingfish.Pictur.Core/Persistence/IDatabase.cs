﻿using System.Collections.Generic;
using Ceilingfish.Pictur.Core.Events;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public interface IDatabase
    {
        IEnumerable<ManagedDirectory> ManagedDirectories { get; }

        IEnumerable<File> Files { get; } 

        bool Add(ManagedDirectory directory);

        void Remove(ManagedDirectory directory);

        File GetFileByPath(string path);

        File GetFileByCheckSum(string checksum);
    }
}
