using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core
{
    public class ConsolePlugin : IHarmonizer
    {
        public void OnNewFile(File file)
        {
            Console.WriteLine("Uploading {0}",file.Path);
        }

        public void OnFileRemoved(File file)
        {
            Console.WriteLine("Removing {0}", file.Path);
        }

        public void OnFileChanged(File file)
        {
            Console.WriteLine("Modified {0}", file.Path);
        }
    }
}
