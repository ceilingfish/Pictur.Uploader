using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ceilingfish.Pictur.Core.Model;

namespace Ceilingfish.Pictur.Core.Api
{
    interface IPlugin
    {
        void OnFileCreated(File file);

        void OnFileUpdated(File file);

        void OnFileDeleted(File file);
    }
}
