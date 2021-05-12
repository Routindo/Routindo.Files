using System.Collections.Generic;
using Routindo.Contract.Attributes;

namespace Routindo.Plugins.Files.Components.Actions.Rename
{
    public static class RenameFileActionExecutionArgs
    {
        [ArgumentInfo("Source File Path", true, typeof(string),
            "The path of the file to rename")]
        public const string SourceFilePath = nameof(SourceFilePath);
    }
}