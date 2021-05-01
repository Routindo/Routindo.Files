using System;
using System.IO;
using System.Linq;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;
using Routindo.Contract.Watchers;

namespace Routindo.Plugins.Files.Components.Watchers
{
    [ResultArgumentsClass(typeof(FilesWatcherResultArguments))]
    [PluginItemInfo(ComponentUniqueId, nameof(FilesWatcher),
        "Watcher directory and reports new created files with a specific pattern.", Category = "Files", FriendlyName = "Watch Directory")]
    public class FilesWatcher : FilesSelector, IWatcher
    {
        public const string ComponentUniqueId = "DEF4D63F-B9B0-4525-BA94-663491DCE04A";

        public string Id { get; set; }

        protected override ILoggingService Logger => this.LoggingService;

        public ILoggingService LoggingService { get; set; }

        public WatcherResult Watch()
        {
            try
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    LoggingService.Warn($"({Id}) Creating directory {DirectoryPath} because it doesn't exist.");
                    var directoryInfo = Directory.CreateDirectory(DirectoryPath);
                    LoggingService.Debug($"({Id}) Directory {directoryInfo.Name} created successfully");
                }

                var sortedFiles = this.Select();

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
                LoggingService.Error(exception);
                return WatcherResult.NotFound;
            }
        }
    }
}