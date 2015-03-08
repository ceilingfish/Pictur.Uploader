using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;

namespace Ceilingfish.Pictur.Core.Flickr
{
    public class FlickrPipeline : ExecutorChain
    {
        private readonly IDatabase _db;

        public FlickrPipeline(IDatabase db)
        {
            _db = db;
            Add(new UploadNewImageExecutor(db));

        }

        public override void Execute(ExecutorContext op)
        {
            if (!_db.Settings.Flickr.IsActive)
                return;
            base.Execute(op);
        }
    }
}
