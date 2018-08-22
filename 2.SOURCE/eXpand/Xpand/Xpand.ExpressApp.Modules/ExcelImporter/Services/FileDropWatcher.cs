﻿using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;

namespace Xpand.ExpressApp.ExcelImporter.Services{
    /// <summary>
    ///     An observable abstraction to monitor for files dropped into a directory
    /// </summary>
    public class FileDropWatcher : IDisposable{
        private readonly string _path;
        private readonly Subject<FileDropped> _pollResults = new Subject<FileDropped>();
        private readonly SearchOption _searchOption;
        private readonly ObservableFileSystemWatcher _watcher;

        public FileDropWatcher(string path, string filter, SearchOption searchOption = SearchOption.TopDirectoryOnly){
            _searchOption = searchOption;
            var includeSubdirectories = searchOption == SearchOption.AllDirectories;
            _path = path;
            _watcher = new ObservableFileSystemWatcher(w => {
                w.Path = path;
                w.Filter = filter;
                // note: filtering on changes can help reduce excessive notifications, make sure to verify any changes with integration tests
                w.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                w.IncludeSubdirectories = includeSubdirectories;
            });

            var renames = _watcher.Renamed.Select(r => new FileDropped(r));
            var creates = _watcher.Created.Select(c => new FileDropped(c));
            var changed = _watcher.Changed.Select(c => new FileDropped(c));

            Dropped = creates
                .Merge(renames)
                .Merge(changed)
                .Merge(_pollResults)
                .Where(dropped => Regex.IsMatch(dropped.Name, filter));
        }

        public IObservable<FileDropped> Dropped{ get; }

        public void Dispose(){
            _watcher.Dispose();
        }

        public void Start(){
            _watcher.Start();
        }

        public void Stop(){
            _watcher.Stop();
        }

        /// <summary>
        ///     Use this to scan for files and raise dropped events for any results.
        ///     This is great to use right after starting the watcher to find existing files.
        ///     Existing files will trigger dropped events through the Dropped stream.
        /// </summary>
        public void PollExisting(){
            
            foreach (var existingFile in Directory.EnumerateFiles(_path,"*.*",_searchOption))
                _pollResults.OnNext(new FileDropped(existingFile));
        }
    }

    public class FileDropped{
        public FileDropped(){
        }

        public FileDropped(FileSystemEventArgs fileEvent){
            Name = fileEvent.Name;
            FullPath = fileEvent.FullPath;
        }

        public FileDropped(string filePath){
            Name = Path.GetFileName(filePath);
            FullPath = filePath;
        }

        public string Name{ get; set; }
        public string FullPath{ get; set; }
    }
}