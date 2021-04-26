namespace Routindo.Plugins.Files.Components.Actions.Move
{
    public static class MoveFileActionExecutionArgs
    {
        public const string SourceFilePaths = nameof(SourceFilePaths);
    }
     
    public static class MoveFileActionInstanceArgs
    {
        public const string DestinationDirectory = nameof(DestinationDirectory);
        public const string DestinationFileName = nameof(DestinationFileName); 
        public const string SourceFilePath = nameof(SourceFilePath); 
        public const string DestinationExtension = nameof(DestinationExtension);
        public const string DestinationPrefix = nameof(DestinationPrefix);
    }
}