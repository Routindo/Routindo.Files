using System;
using System.IO;
using System.Linq;
using NLog;
using Umator.Contract;

namespace Umator.Plugins.Files.Components.Watchers
{
    [ResultArgumentsClass(typeof(FilesWatcherResultArguments))]
    [PluginItemInfo(FilesWatcher.ComponentUniqueId, "Files Watcher",
        "Watcher directory and reports new created files with a specific pattern.")]
    public class FilesWatcher : IWatcher
    {
        public const string ComponentUniqueId = "DEF4D63F-B9B0-4525-BA94-663491DCE04A";
        /// <summary>
        ///     The logger
        /// </summary>
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Gets or sets the directory path.
        /// </summary>
        /// <value>
        ///     The directory path.
        /// </value>
        [Argument(FilesWatcherArguments.Directory, true)]
        public string DirectoryPath { get; set; }

        /// <summary>
        ///     Gets or sets the search pattern.
        /// </summary>
        /// <value>
        ///     The search pattern.
        /// </value>
        [Argument(FilesWatcherArguments.Pattern, true)]
        public string SearchPattern { get; set; }

        /// <summary>
        ///     Gets or sets the maximum files.
        /// </summary>
        /// <value>
        ///     The maximum files.
        /// </value>
        [Argument(FilesWatcherArguments.MaximumFiles, true)]
        public int MaximumFiles { get; set; }

        public string Id { get; set; }

        public WatcherResult Watch()
        {
            try
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    _logger.Warn($"({Id}) Creating directory {DirectoryPath} because it doesn't exist.");
                    var directoryInfo = Directory.CreateDirectory(DirectoryPath);
                    _logger.Debug($"({Id}) Directory {directoryInfo.Name} created successfully");
                }

                var sortedFiles = new DirectoryInfo(DirectoryPath)
                    .GetFiles(SearchPattern, SearchOption.TopDirectoryOnly)
                    .OrderBy(f => f.LastWriteTime)
                    .Select(e => e.FullName)
                    .Take(MaximumFiles)
                    .ToList();

                if (sortedFiles.Any())
                    return new WatcherResult
                    {
                        Result = true,
                        WatchingArguments = new ArgumentCollection((FilesWatcherResultArguments.Files, sortedFiles))
                    };

                return WatcherResult.NotFound;
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return WatcherResult.NotFound;
            }
        }
    }
}