using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class ExceptionHandlingExecutor : ExceptionHandlingExecutor<ExecutorContext>, IExecutor
    {
        public ExceptionHandlingExecutor(IExecutor<ExecutorContext> exe)
            : base(exe)
        {
        }
    }

    public class ExceptionHandlingExecutor<T> : IExecutor<T>
        where T : ExecutorContext
    {
        public EventHandler<ExecutorExceptionArgs> Exception;

        private readonly IExecutor<T> _internalExecutor;

        public ExceptionHandlingExecutor(IExecutor<T> exe)
        {
            if (exe == null)
                throw new ArgumentException("non null executor");
            _internalExecutor = exe;
        }

        public void Execute(T op)
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
