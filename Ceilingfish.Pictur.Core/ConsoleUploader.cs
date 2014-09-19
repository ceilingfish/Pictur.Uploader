using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core
{
    public class ConsoleUploader
    {
        public void Upload(File file)
        {
            Console.WriteLine("Uploading {0}",file.Path);
        }

        public void Delete(File file)
        {
            Console.WriteLine("Removing {0}", file.Path);
        }
    }
}
