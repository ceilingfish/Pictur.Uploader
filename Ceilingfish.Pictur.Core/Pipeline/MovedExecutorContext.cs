using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class MovedExecutorContext : ExecutorContext
    {
        public readonly string OldPath, NewPath;

        public MovedExecutorContext(Models.File file, string old, string newPath)
            : base(file, FileOperationType.Moved)
        {
            OldPath = old;
            NewPath = newPath;
        }
    }
}