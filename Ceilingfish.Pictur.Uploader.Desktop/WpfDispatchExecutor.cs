using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Pipeline;

namespace Ceilingfish.Pictur.Uploader.Desktop
{
    public class WpfDispatchExecutor : IExecutor
    {
        private readonly System.Windows.Threading.Dispatcher _dispatcher;
        private readonly IExecutor _executor;

        public WpfDispatchExecutor(System.Windows.Threading.Dispatcher dispatcher, FileStatus executor)
        {
            _dispatcher = dispatcher;
            _executor = executor;
        }
        public void Execute(FileOperation op)
        {
            _dispatcher.Invoke(() => _executor.Execute(op));
        }
    }
}
