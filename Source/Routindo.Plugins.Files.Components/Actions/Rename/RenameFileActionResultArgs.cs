using Routindo.Contract.Attributes;

namespace Routindo.Plugins.Files.Components.Actions.Rename
{
    public static class RenameFileActionResultArgs
    {
        [ArgumentInfo("Source File Path", true, typeof(string),
            "The path of the file to rename")]
        public const string SourceFilePath = nameof(SourceFilePath);

        [ArgumentInfo("Destination File Path", true, typeof(string),
            "The new path of the file renamed")]
        public const string DestinationFilePath = nameof(DestinationFilePath);
    }
}