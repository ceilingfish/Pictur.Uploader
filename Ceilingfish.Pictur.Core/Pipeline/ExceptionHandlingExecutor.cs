using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class ExceptionHandlingExecutor : IExecutor
    {
        public EventHandler<ExecutorExceptionArgs> Exception;

        private readonly IExecutor _internalExecutor;

        public ExceptionHandlingExecutor(IExecutor exe)
        {
            if (exe == null)
                throw new ArgumentException("non null executor");
            _internalExecutor = exe;
        }

        public void Execute(ExecutorContext op)
        {
            try
            {
                _internalExecutor.Execute(op);
            }
            catch (Exception e)
            {
                if (Exception != null)
                    Exception(this, new ExecutorExceptionArgs(op, e));
            }
        }
    }
}
