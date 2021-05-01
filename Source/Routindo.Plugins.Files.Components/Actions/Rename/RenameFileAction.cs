using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Routindo.Contract.Actions;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Exceptions;
using Routindo.Contract.Services;

namespace Routindo.Plugins.Files.Components.Actions.Rename
{
    [PluginItemInfo(ComponentUniqueId, nameof(RenameFileAction),
        "Rename a an existing file to a new name, or simply append a prefix or change the current extension", Category = "Files", FriendlyName = "Rename File")]
    [ExecutionArgumentsClass(typeof(RenameFileActionExecutionArgs))]
    [ResultArgumentsClass(typeof(RenameFileActionResultArgs))]
    public class RenameFileAction: IAction
    {
        public const string ComponentUniqueId = "3EF330CD-5B38-42D3-97BE-43AE10A0CBE2";

        public string Id { get; set; } 
        public ILoggingService LoggingService { get; set; }

        [Argument(RenameFileActionArgs.DestinationFileName, false)] public string DestinationFileName { get; set; }

        [Argument(RenameFileActionArgs.SourceFilePath, false)] public string SourceFilePath { get; set; }

        [Argument(RenameFileActionArgs.DestinationExtension, false)] public string DestinationExtension { get; set; }

        [Argument(RenameFileActionArgs.DestinationPrefix, false)] public string DestinationPrefix { get; set; }

        public ActionResult Execute(ArgumentCollection arguments)
        {
            string filePath = null;
            try
            {
                if (arguments.HasArgument(RenameFileActionArgs.SourceFilePath))
                {
                    if (arguments[RenameFileActionArgs.SourceFilePath] is List<string> castedFilePaths)
                    {
                        if (castedFilePaths.Count() > 1)
                        {
                            throw new Exception($"Operation not allowed: Got multiple files to be renamed.");
                        }
                        if (!castedFilePaths.Any())
                            throw new Exception($"Operation not allowed: Got empty list of files to be renamed.");
                        filePath = castedFilePaths.Single();
                    }
                    else
                    {
                        filePath = arguments.GetValue<string>(RenameFileActionArgs.SourceFilePath);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(SourceFilePath))
                        filePath = SourceFilePath;
                }

                if(string.IsNullOrWhiteSpace(filePath))
                    throw new MissingArgumentException(RenameFileActionArgs.SourceFilePath);

                if(!Path.IsPathRooted(filePath))
                    throw new FormatException($"Source file has incorrect format, expected Rooted Path: {filePath}");

                // Files must exist
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found {filePath}");

                if (string.IsNullOrWhiteSpace(DestinationFileName) && string.IsNullOrWhiteSpace(DestinationPrefix) &&
                    string.IsNullOrWhiteSpace(DestinationExtension))
                {
                    throw new MissingArgumentException(RenameFileActionArgs.DestinationFileName);
                }

                var directory = Path.GetDirectoryName(filePath);
                if(string.IsNullOrWhiteSpace(directory))
                    throw new Exception($"Unable to get directory name from file {filePath}");

                var fileName = Path.GetFileName(filePath);
                if (!string.IsNullOrWhiteSpace(DestinationFileName)) fileName = DestinationFileName;
                else
                {
                    if (!string.IsNullOrEmpty(DestinationExtension))
                    {
                        fileName = Path.ChangeExtension(fileName, DestinationExtension);
                    }

                    if (!string.IsNullOrEmpty(DestinationPrefix))
                    {
                        fileName = DestinationPrefix + fileName;
                    }
                }

                if(string.IsNullOrWhiteSpace(fileName))
                    throw new Exception($"FileName not resolved, Cannot rename file");
                var destinationPath = Path.Combine(directory, fileName); 

                if(File.Exists(destinationPath))
                    throw new Exception($"Another file with the same name already exists {destinationPath}");

                File.Move(filePath, destinationPath);
                LoggingService.Trace($"File ({filePath}) renamed successfully to path ({destinationPath})");

                return ActionResult.Succeeded().WithAdditionInformation(ArgumentCollection.New()
                    .WithArgument(RenameFileActionResultArgs.SourceFilePath, filePath)
                    .WithArgument(RenameFileActionResultArgs.DestinationFilePath, destinationPath)
                );
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return new ActionResult(false)
                {
                    AttachedException = exception, 
                    AdditionalInformation = ArgumentCollection.New().WithArgument(RenameFileActionResultArgs.SourceFilePath, filePath)
                };
            }
        }
    }
}
