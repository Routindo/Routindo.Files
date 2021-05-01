using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Routindo.Contract.Actions;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Exceptions;
using Routindo.Contract.Services;
using Routindo.Plugins.Files.Components.Actions.Move;

namespace Routindo.Plugins.Files.Components.Actions.Copy
{
    [PluginItemInfo(ComponentUniqueId, name:nameof(CopyFilesAction),
        "Copy one or more files to a specific directory", Category = "Files", FriendlyName = "Copy Files")]
    [ExecutionArgumentsClass(typeof(CopyFilesActionExecutionArgs))]
    public class CopyFilesAction: IAction
    {
        public const string ComponentUniqueId = "22A3DE70-0FF5-480A-9741-BF88215D0179";
        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        [Argument(CopyFilesActionArgs.DestinationDirectory, true)] public string DestinationDirectory { get; set; }
        [Argument(CopyFilesActionArgs.SourceFilePath, false)] public string SourceFilePath { get; set; }
        public ActionResult Execute(ArgumentCollection arguments)
        {
            try
            {
                List<string> filePaths = new List<string>();

                if (arguments.HasArgument(MoveFileActionExecutionArgs.SourceFilePaths))
                {
                    if (arguments[MoveFileActionExecutionArgs.SourceFilePaths] is List<string> castedFilePaths)
                    {
                        filePaths.AddRange(castedFilePaths);
                    }
                    else
                    {
                        throw new ArgumentsValidationException(
                            $"unable to cast argument value into list of string. key({MoveFileActionExecutionArgs.SourceFilePaths})");
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(SourceFilePath))
                        filePaths.Add(SourceFilePath);
                }

                // Files must exist
                if (filePaths.Any(f => !File.Exists(f)))
                    throw new FileNotFoundException("File not found", filePaths.First(e => !File.Exists(e)));

                foreach (var sourcePath in filePaths)
                {
                    var fileName = Path.GetFileName(sourcePath);

                    var destinationPath = Path.Combine(DestinationDirectory, fileName);

                    // File must not exist
                    if (File.Exists(destinationPath))
                        throw new Exception($"({destinationPath}) File already exist");

                    File.Copy(sourcePath, destinationPath);
                    LoggingService.Info($"File ({sourcePath}) copied successfully to path ({destinationPath})");
                }

                return ActionResult.Succeeded();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return new ActionResult(false)
                {
                    AttachedException = exception
                };
            }
        }
    }
}
