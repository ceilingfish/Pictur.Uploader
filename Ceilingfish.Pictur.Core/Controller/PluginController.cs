using System;
using System.IO;
using File = Ceilingfish.Pictur.Core.Model.File;

namespace Ceilingfish.Pictur.Core.Controller
{
    public class PluginController : IController
    {
        #region File Events

        internal void OnFileCreated(object sender, FileCreatedArgs args)
        {
        }

        internal void OnFileModified(object sender, FileModifiedArgs args)
        {
            
        }

        internal void OnFileDeleted(object sender, FileDeletedArgs args)
        {
            
        }

        #endregion

        public void FileCreated(FileInfo file)
        {
            Console.WriteLine("Created: {0}",file.FullName);
        }

        public void FileModified(File file)
        {
            Console.WriteLine("Modified: {0}", file.Path);
        }

        public void FileDeleted(File file)
        {
            Console.WriteLine("Modified: {0}", file.Path);
        }
    }
}