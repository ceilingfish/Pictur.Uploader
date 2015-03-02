using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.FileSystem
{
    public interface IFileScanEnumerator
    {
        IEnumerable<Models.File> Scan();
    }
}
