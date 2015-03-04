using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class ExecutorExceptionArgs : EventArgs
    {
        public readonly ExecutorContext Context;
        public readonly Exception Exception;

        public ExecutorExceptionArgs(ExecutorContext ctx, Exception e)
        {
            Context = ctx;
            Exception = e;
        }
    }
}
