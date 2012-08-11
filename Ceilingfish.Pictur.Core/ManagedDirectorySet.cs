using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Ceilingfish.Pictur.Core.Properties;

namespace Ceilingfish.Pictur.Core
{
    public class ManagedDirectorySet : ObservableCollection<DirectoryInfo>
    {
        private readonly Settings _settings;

        public ManagedDirectorySet()
        : this(Settings.Default)
        {}

        internal ManagedDirectorySet(Settings settings)
            : this(settings.ManagedDirectories)
        {
            _settings = settings;
        }

        public ManagedDirectorySet(string paths)
        {
            if(paths == null)
                throw new ArgumentException();

            var directories = paths.Split(';')
                .Where(s => !String.IsNullOrEmpty(s))
                .Select(d => new DirectoryInfo(d));

            foreach (var directoryInfo in directories)
            {
                Add(directoryInfo);
            }
        }

        public void Add(string path)
        {
            Add(new DirectoryInfo(path));
        }

        public void Save()
        {
            if(_settings != null)
                _settings.Save();
        }
    }
}
