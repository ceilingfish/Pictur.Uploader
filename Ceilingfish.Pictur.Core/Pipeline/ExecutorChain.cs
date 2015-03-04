using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class ExecutorChain : IExecutor
    {
        private readonly List<IExecutor> _executors = new List<IExecutor>(); 

        public void Execute(FileOperation op)
        {
            
        }

        private IEnumerable<Exception> ExecuteChain(FileOperation op)
        {
            foreach (var executor in _executors)
            {
                
            }
            try
            {

            }
            catch (Exception e)
            {
                yield return e;
                throw;
            }
        }

        public void Add(IExecutor executor)
        {
            _executors.Add(executor);
        }
    }
}
