using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public interface IExecutor<T>
        where T : ExecutorContext
    {
        void Execute(T context);
    }

    public interface IExecutor : IExecutor<ExecutorContext>
    {
    }
}
