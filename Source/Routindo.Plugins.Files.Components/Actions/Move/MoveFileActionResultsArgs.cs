using System.Collections.Generic;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.Files.Components.Actions.Move
{
    public static class MoveFileActionResultsArgs
    {
        [ArgumentInfo("Source Files Paths", true, typeof(List<string>),
            "The list of files to move")]
        public const string SourceFilesPaths = nameof(SourceFilesPaths);

        [ArgumentInfo("Moved Files Paths", true, typeof(List<string>),
            "The list of files paths moved successfully")]
        public const string MovedFilesPaths = nameof(MovedFilesPaths);

        [ArgumentInfo("Failed Files Paths", true, typeof(List<string>),
            "The list of files paths failed to be moved.")]
        public const string FailedFilesPaths = nameof(FailedFilesPaths);
    }
}