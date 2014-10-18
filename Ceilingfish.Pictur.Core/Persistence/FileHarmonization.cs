using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class FileHarmonization
    {
        public string Id { get; set; }

        public string FileId { get; set; }

        public HarmonizationState State { get; set; }
    }
}
