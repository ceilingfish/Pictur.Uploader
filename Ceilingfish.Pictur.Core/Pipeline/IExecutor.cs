using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public interface IExecutor
    {
        void Execute(ExecutorContext op);
    }
}
