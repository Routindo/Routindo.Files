using System.Collections.Generic;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.Files.Components.Actions.Delete
{
    public static class DeleteFilesActionResultsArgs
    {
        [ArgumentInfo("Deleted Files Paths", true, typeof(List<string>),
            "The list of files deleted successfully")]
        public const string DeletedFiles = nameof(DeletedFiles);
    }
}