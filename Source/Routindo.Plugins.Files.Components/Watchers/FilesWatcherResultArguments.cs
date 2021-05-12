using System.Collections.Generic;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.Files.Components.Watchers
{
    public static class FilesWatcherResultArguments
    {
        [ArgumentInfo("Files Collection", true, typeof(List<string>),
            "List of Files sorted basing on the criteria defined in the Watcher Configuration")]
        public const string Files = nameof(Files);
    }
}