using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core
{
    public class ManagedDirectoryScanner
    {
        public event EventHandler<FileDetectedArgs> Detected;

        private Task _asyncTask;
        private readonly CancellationTokenSource _cancel = new CancellationTokenSource();
        private readonly IEnumerable<ManagedDirectory> _directories;
        
        public ManagedDirectoryScanner(IEnumerable<ManagedDirectory> dir)
        {
            _directories = dir;
        }

        public Task ScanAsync()
        {
            if(_asyncTask == null)
                _asyncTask = Task.Run(new Action(Scan),_cancel.Token);

            return _asyncTask;
        }

        public void Scan()
        {
            foreach (var detectedFile in List())
            {
                if (Detected != null)
                    Detected(this, detectedFile);
            }
        }

        public IEnumerable<FileDetectedArgs> List()
        {
            foreach (var dir in _directories)
            {
                foreach(var file in System.IO.Directory.EnumerateFiles(dir.Path,"*",System.IO.SearchOption.AllDirectories))
                {
                    if (_cancel.IsCancellationRequested)
                        yield break;

                    yield return new FileDetectedArgs(dir,file);
                }
            }
        }

        public void Cancel()
        {
            _cancel.Cancel();
        }
    }
}
