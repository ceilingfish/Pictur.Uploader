using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core
{
    public class ConsoleUploader : BaseUploader
    {
        public ConsoleUploader(IDatabase db) : base(db)
        {
        }

        protected override void Upload(File file)
        {
            Console.WriteLine("Uploading {0}",file.Path);
        }

        protected override void Delete(File file)
        {
            Console.WriteLine("Removing {0}", file.Path);
        }
    }
}
