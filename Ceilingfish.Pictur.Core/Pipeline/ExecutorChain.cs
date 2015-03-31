using System;
using System.Collections;
using System.Collections.Generic;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class ExecutorChain : ExecutorChain<ExecutorContext> { }

    public class ExecutorChain<T> : IExecutor<T>, IEnumerable<IExecutor<T>>
        where T : ExecutorContext
    {
        private readonly List<IExecutor<T>> _executors = new List<IExecutor<T>>();

        public virtual void Execute(T op)
        {
            List<Exception> errors;
            if (!ExecuteChain(op, out errors))
                throw new AggregateException(errors);
        }

        private bool ExecuteChain(T op, out List<Exception> errors)
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

        public void Add(IExecutor<T> executor)
        {
            _executors.Add(executor);
        }

        public IEnumerator<IExecutor<T>> GetEnumerator()
        {
            return _executors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
