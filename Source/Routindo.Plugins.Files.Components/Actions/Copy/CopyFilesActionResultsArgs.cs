using System.Collections.Generic;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.Files.Components.Actions.Copy
{
    public static class CopyFilesActionResultsArgs
    {
        [ArgumentInfo("Source Files Paths", true, typeof(List<string>),
            "The list of original files paths copied")]
        public const string SourceFilePaths = nameof(SourceFilePaths);

        [ArgumentInfo("Copied Files Paths", true, typeof(List<string>),
            "The list of the new files destination paths after been copied")]
        public const string CopiedFilesPaths = nameof(CopiedFilesPaths);
    }
}