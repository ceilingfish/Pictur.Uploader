using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class MovedFileOperation : FileOperation
    {
        public readonly string OldPath, NewPath;

        public MovedFileOperation(Models.File file, string old, string newPath)
            :base(file, FileOperationType.Moved)
        {
            OldPath = old;
            NewPath = newPath;
        }
    }
}