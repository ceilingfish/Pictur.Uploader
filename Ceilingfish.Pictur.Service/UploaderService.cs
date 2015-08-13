using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using Ceilingfish.Pictur.Core;
using Ceilingfish.Pictur.Core.FileSystem;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Service
{
    public partial class UploaderService : ServiceBase
    {

        private readonly CancellationTokenSource _token = new CancellationTokenSource();
        private Uploader _uploaderExecution;

        public UploaderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _uploaderExecution = new Uploader(_token.Token);
            _uploaderExecution.Start();
        }

        protected override void OnStop()
        {
            _token.Cancel();
            _uploaderExecution.Stop();
            base.OnStop();
        }
    }
}