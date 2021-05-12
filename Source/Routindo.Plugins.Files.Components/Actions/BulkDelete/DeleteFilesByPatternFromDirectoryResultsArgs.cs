using System.Collections.Generic;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.Files.Components.Actions.BulkDelete
{
    public static class DeleteFilesByPatternFromDirectoryResultsArgs
    {
        [ArgumentInfo("Deleted Files Paths", true, typeof(List<string>),
            "The list of files deleted successfully")]
        public const string DeletedFiles = nameof(DeletedFiles);

        [ArgumentInfo("Failed Files Paths", true, typeof(List<string>),
            "The list of files failed to be deleted")]
        public const string FailedFiles = nameof(FailedFiles); 
    }
}