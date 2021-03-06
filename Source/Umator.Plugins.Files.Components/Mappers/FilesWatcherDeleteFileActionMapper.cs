﻿using System.Collections.Generic;
using Umator.Contract;
using Umator.Contract.Services;
using Umator.Plugins.Files.Components.Actions.DeleteFiles;
using Umator.Plugins.Files.Components.Watchers;

namespace Umator.Plugins.Files.Components.Mappers
{
    [PluginItemInfo("F587A697-4F28-4A44-BCB6-9519FE31028E", "Deleted Watched Files Args Mapper",
        "Map arguments of " + nameof(FilesWatcher) + "to arguments of " + nameof(DeleteFilesAction))]
    public class FilesWatcherDeleteFileActionMapper : IArgumentsMapper
    {
        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        public ArgumentCollection Map(ArgumentCollection arguments)
        {
            if (!arguments.HasArgument(FilesWatcherResultArguments.Files) ||
                !(arguments[FilesWatcherResultArguments.Files] is List<string>))
                return null;

            return new ArgumentCollection(
                (DeleteFilesActionExecutionArgs.FilesPaths, arguments[FilesWatcherResultArguments.Files])
            );
        }
    }
}