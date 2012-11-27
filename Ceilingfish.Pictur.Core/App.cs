using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ceilingfish.Pictur.Core.Controller;

namespace Ceilingfish.Pictur.Core
{
    public class App
    {
        private PersistenceController _database;

        public ManagedDirectoryController ManagedDirectories;

        public PluginController Plugins;

        public void Start()
        {
            Plugins = new PluginController();
            _database = new PersistenceController();
            
            var db = _database.LoadDatabase();
            
            ManagedDirectories = new ManagedDirectoryController(db);

            ManagedDirectories.Created += Plugins.OnFileCreated;
            ManagedDirectories.Modified += Plugins.OnFileModified;
            ManagedDirectories.Deleted += Plugins.OnFileDeleted;

            ManagedDirectories.Start();
        }
    }
}
