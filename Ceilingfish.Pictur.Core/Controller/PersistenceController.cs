using System;
using System.Data.Metadata.Edm;
using System.IO;
using System.Reflection;

namespace Ceilingfish.Pictur.Core.Controller
{
    public class PersistenceController : IController
    {

        public void LoadDatabase()
        {
            var localAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var sqlFilePath = Path.Combine(localAppFolder, "Pictur", "pictur.db");

        }

        private string GetSchemaFile()
        {
            var executable = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);

            var exeFolder = Path.GetDirectoryName(executable);

            return Path.Combine(exeFolder, "schema.sql");
        }

        
    }
}
