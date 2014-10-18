using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core
{
    public interface IHarmonizer
    {
        void OnNewFile(File file);
        
        void OnFileRemoved(File file);
        
        void OnFileChanged(File file);
    }
}
