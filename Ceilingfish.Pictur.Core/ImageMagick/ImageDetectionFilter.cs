using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Pipeline;
using ImageMagick;
using Serilog;

namespace Ceilingfish.Pictur.Core.ImageMagick
{
    public class ImageDetectionFilter : IExecutor
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<ImageDetectionFilter>();

        private readonly IExecutor _subExecutor;

        public ImageDetectionFilter(IExecutor executor)
        {
            _subExecutor = executor;
        }

        public void Execute(ExecutorContext op)
        {
            if (op.Type == FileOperationType.Removed)
                return;

            try
            {
                using (var image = new MagickImage(op.File.Path))
                {
                    _subExecutor.Execute(op);
                }
            }
            catch (MagickMissingDelegateErrorException)
            {
                Log.Verbose("Ignoring non-image file {File}", op.File.Path);
            }

        }
    }
}
