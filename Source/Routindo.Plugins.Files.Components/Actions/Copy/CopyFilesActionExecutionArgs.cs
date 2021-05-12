using System.Collections.Generic;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.Files.Components.Actions.Copy
{
    public static class CopyFilesActionExecutionArgs
    {
        [ArgumentInfo("Files Paths", true, typeof(List<string>),
            "The list of files to copy")]
        public const string SourceFilePaths = nameof(SourceFilePaths);
    }
}