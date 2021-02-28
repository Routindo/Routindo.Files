using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using Umator.Contract;

namespace Umator.Plugins.Files.Components.Actions.MoveFiles
{
    [PluginItemInfo(ComponentUniqueId, "File Mover",
        "Move a specific file to a specific directory")]
    [ExecutionArgumentsClass(typeof(MoveFileActionExecutionArgs))]
    public class MoveFileAction : IAction
    {
        public const string ComponentUniqueId = "02170633-B58A-4429-AEC1-B813DAA87BF5"; 
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();


        public string Id { get; set; }

        [Argument(MoveFileActionInstanceArgs.DestinationDirectory, true)] public string DestinationDirectory { get; set; }

        [Argument(MoveFileActionInstanceArgs.DestinationExtension, false)] public string DestinationExtension { get; set; }

        [Argument(MoveFileActionInstanceArgs.DestinationPrefix, false)] public string DestinationPrefix { get; set; }

        [Argument(MoveFileActionInstanceArgs.DestinationFileName, false)] public string DestinationFileName { get; set; } 

        [Argument(MoveFileActionInstanceArgs.SourceFilePath, false)] public string SourceFilePath { get; set; }  

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
                if (filePaths.Any(f=> !File.Exists(f)))
                    throw new FileNotFoundException("File not found", filePaths.First(e=> !File.Exists(e)));

                foreach (var sourcePath in filePaths)
                {
                    var fileName = Path.GetFileName(sourcePath);
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

                    var destinationPath = Path.Combine(DestinationDirectory, fileName);

                    // File must not exist
                    if (File.Exists(destinationPath))
                        throw new Exception($"({destinationPath}) File already exist");

                    File.Move(sourcePath, destinationPath);
                    _logger.Info($"File ({sourcePath}) moved successfully to path ({destinationPath})");
                }
                
                return ActionResult.Succeeded();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return new ActionResult(false)
                {
                    AttachedException = exception
                };
            }
        }
    }

    
}