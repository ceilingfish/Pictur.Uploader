using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;
using Serilog;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class FlickrPipeline : IExecutor
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<FlickrPipeline>();

        private readonly IDatabase _db;

        private readonly ExecutorChain<FlickrContext> _activeChain;
        private readonly ExecutorChain<FlickrContext> _invalidTokenChain;

        public FlickrPipeline(IDatabase db)
        {
            _db = db;
            _activeChain = new ExecutorChain<FlickrContext>
            {
                new ResolveUploadState(db),
                new UploadNewImageExecutor(db)
            };
            _invalidTokenChain = new ExecutorChain<FlickrContext>
            {

            };

        }

        public void Execute(ExecutorContext op)
        {
            var status = _db.Settings.Flickr.Status;
            if (status == FlickrStatus.Disabled)
                return;

            var chain = status == FlickrStatus.TokenInvalid ? _invalidTokenChain : _activeChain;
            try
            {
                using (var context = new FlickrContext(op.File, op.FileOperation))
                {
                    chain.Execute(context);
                }
            }
            catch (ApiException e)
            {
                if (e.Error != null && e.Error.Code == 98)
                {
                    _db.Settings.Flickr.Status = FlickrStatus.TokenInvalid;
                }
                else
                {
                    Log.Debug(e, "Problem uploading picture {0}", op.File);
                }
            }
        }
    }
}
