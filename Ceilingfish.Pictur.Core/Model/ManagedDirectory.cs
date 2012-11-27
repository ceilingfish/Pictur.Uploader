using System;
using System.IO;
using System.Windows;

namespace Ceilingfish.Pictur.Core.Model
{
    public class ManagedDirectory
    {
        internal readonly DirectoryInfo Directory;
        
        public ManagedDirectory(string s)
        {
            Directory = new DirectoryInfo(s);
            
        }
    }
}
