using System;
using System.Collections;
using System.Collections.Generic;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class ExecutorChain : IExecutor, IEnumerable<IExecutor>
    {
        private readonly List<IExecutor> _executors = new List<IExecutor>();

        public virtual void Execute(ExecutorContext op)
        {
            List<Exception> errors;
            if (!ExecuteChain(op, out errors))
                throw new AggregateException(errors);
        }

        private bool ExecuteChain(ExecutorContext op, out List<Exception> errors)
        {
            errors = null;
            foreach (var executor in _executors)
            {
                try
                {
                    executor.Execute(op);
                }
                catch (Exception e)
                {
                    if (errors == null)
                        errors = new List<Exception>();
                    errors.Add(e);
                }
            }

            return errors == null;
        }

        public void Add(IExecutor executor)
        {
            _executors.Add(executor);
        }

        public IEnumerator<IExecutor> GetEnumerator()
        {
            return _executors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
