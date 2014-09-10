using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Ceilingfish.Pictur.Core.Helpers;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Controller
{
    public class ManagedDirectoryController : IController
    {
        public event EventHandler<FileCreatedArgs> Created;
        public event EventHandler<FileDeletedArgs> Deleted;
        public event EventHandler<FileModifiedArgs> Modified;

        private class ManagedDirectoryItem
        {
            internal readonly ManagedDirectory Directory;
            internal readonly FileSystemWatcher Watcher;

            internal ManagedDirectoryItem(ManagedDirectory dir)
            {
                Directory = dir;
                //Watcher = new FileSystemWatcher(dir.Directory.FullName);
            }
        }

        private readonly PersistenceController _persistence;
        private readonly List<ManagedDirectoryItem> _directories = new List<ManagedDirectoryItem>(); 

        public ManagedDirectoryController(PersistenceController db)
        {
            _persistence = db;
        }

        internal void Start()
        {
            foreach (var managedDirectory in _persistence.ManagedDirectories)
            {
                var item = new ManagedDirectoryItem(managedDirectory);

                item.Watcher.Created += OnDirectoryItemCreated;
                item.Watcher.Changed += OnDirectoryItemChanged;
                item.Watcher.Deleted += OnDirectoryItemDeleted;
                item.Watcher.Renamed += OnDirectoryItemRenamed;
                _directories.Add(item);

                item.Watcher.EnableRaisingEvents = true;
            }
        }

        #region File System Watchers

        private void OnDirectoryItemCreated(object sender, FileSystemEventArgs e)
        {
            CreateFile(new FileInfo(e.FullPath));
        }

        private void OnDirectoryItemRenamed(object sender, RenamedEventArgs e)
        {
            var oldFile = _persistence.Files.First(f => f.Path.Equals(e.OldFullPath));
            
            if (oldFile == null)
                return;

            ModifiedFile(oldFile,e.FullPath);
        }

        private void OnDirectoryItemDeleted(object sender, FileSystemEventArgs e)
        {
            var f = _persistence.FindFileByPath(e.FullPath);

            DeleteFile(f);
        }

        private void OnDirectoryItemChanged(object sender, FileSystemEventArgs e)
        {
            var file = _persistence.FindFileByPath(e.FullPath);

            ModifiedFile(file,e.FullPath);
        }

        #endregion

        private void CreateFile(FileInfo file)
        {
            var checksum = ChecksumHelper.GetMd5HashFromFile(file.FullName);

            var newFile = new Persistence.File(file)
            {
                Checksum = checksum
            };

            _persistence.Files.AddObject(newFile);

            var vetoed = false;
            if (Created != null)
            {
                var createEvent = new FileCreatedArgs(newFile);
                Created(this, createEvent);

                vetoed = createEvent.Cancel;
            }

            if (vetoed)
            {
                newFile.IsIgnored = true;
                _persistence.Files.AddObject(newFile);
                _persistence.Save();
            }


        }

        private void ModifiedFile(Persistence.File oldFile, string path)
        {
            var newFile = new Persistence.File(new FileInfo(path))
            {
                Checksum = oldFile.Checksum
            };

            if (Modified != null)
                Modified(this, new FileModifiedArgs(oldFile, newFile));

            _persistence.Files.AddObject(newFile);
            _persistence.Save();
        }

        private void DeleteFile(Persistence.File f)
        {
            if (Deleted != null)
                Deleted(this, new FileDeletedArgs(f));

            _persistence.Files.DeleteObject(f);
            _persistence.Save();
        }
        
        private void SubmitDirectory(DirectoryInfo directory)
        {
            foreach (var fileInfo in directory.GetFiles())
            {
                SubmitFile(fileInfo);
            }

            foreach (var directoryInfo in directory.GetDirectories())
            {
                SubmitDirectory(directoryInfo);
            }
        }

        private void SubmitFile(FileInfo path)
        {
            //TODO this needs more configurability
            ThreadPool.QueueUserWorkItem(CheckFile, path);
        }

        private void CheckFile(object fileObj)
        {
            var fileInfo = fileObj as FileInfo;

            var file = _persistence.FindFileByPath(fileInfo.FullName);

            var checksum = ChecksumHelper.GetMd5HashFromFile(fileInfo.FullName);

            var previousFile = _persistence.FindFileByChecksum(checksum);

            //if we found a previous file, then check to see if it exists
            if(previousFile != null)
            {
                //If it's the same file then 
                if (fileInfo.FullName.Equals(previousFile.Path))
                    return;

                ModifiedFile(previousFile, fileInfo.FullName);
            }
            //If we found no cache indicator of a previous file then create a new entry
            else if (file == null)
            {
                CreateFile(fileInfo);
            }
            else if(file.FileLength != fileInfo.Length || !checksum.Equals(file.Checksum))
            {
                ModifiedFile(previousFile, fileInfo.FullName);
            }
        }

        public void AddDirectory(string p)
        {
            
        }
    }
}
