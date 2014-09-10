using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.Events
{
    public delegate void RecordRemoved<in T>(T me);
}
