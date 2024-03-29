﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Routindo.Contract.Actions;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Exceptions;
using Routindo.Contract.Services;

namespace Routindo.Plugins.Files.Components.Actions.Move
{
    [PluginItemInfo(ComponentUniqueId, nameof(MoveFileAction),
        "Move one or more files from one location to a specific directory", Category = "Files", FriendlyName = "Move Files")]
    [ExecutionArgumentsClass(typeof(MoveFileActionExecutionArgs))]
    [ResultArgumentsClass(typeof(MoveFileActionResultsArgs))]
    public class MoveFileAction : IAction
    {
        public const string ComponentUniqueId = "02170633-B58A-4429-AEC1-B813DAA87BF5";


        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        [Argument(MoveFileActionInstanceArgs.DestinationDirectory, true)] public string DestinationDirectory { get; set; }

        [Argument(MoveFileActionInstanceArgs.DestinationExtension, false)] public string DestinationExtension { get; set; }

        [Argument(MoveFileActionInstanceArgs.DestinationPrefix, false)] public string DestinationPrefix { get; set; }

        [Argument(MoveFileActionInstanceArgs.DestinationFileName, false)] public string DestinationFileName { get; set; } 

        [Argument(MoveFileActionInstanceArgs.SourceFilePath, false)] public string SourceFilePath { get; set; }  

        public ActionResult Execute(ArgumentCollection arguments)
        {
            List<string> filePaths = new List<string>();
            List<string> movedFilesPaths = new List<string>();
            List<string> failedFilesPaths = new List<string>();
            try
            {
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
                    try
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
                        {
                            throw new Exception($"({destinationPath}) File already exist");
                        }

                        File.Move(sourcePath, destinationPath);
                        LoggingService.Info($"File ({sourcePath}) moved successfully to path ({destinationPath})");
                        movedFilesPaths.Add(destinationPath);
                    }
                    catch (Exception exception)
                    {
                        LoggingService.Error(exception);
                        failedFilesPaths.Add(sourcePath);
                    }
                }

                return ActionResult.Succeeded().WithAdditionInformation(ArgumentCollection.New()
                    .WithArgument(MoveFileActionResultsArgs.SourceFilesPaths, filePaths)
                    .WithArgument(MoveFileActionResultsArgs.MovedFilesPaths, movedFilesPaths)
                    .WithArgument(MoveFileActionResultsArgs.FailedFilesPaths, failedFilesPaths)
                );
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return ActionResult.Failed(exception).WithAdditionInformation(ArgumentCollection.New()
                    .WithArgument(MoveFileActionResultsArgs.SourceFilesPaths, filePaths)
                    .WithArgument(MoveFileActionResultsArgs.MovedFilesPaths, movedFilesPaths)
                    .WithArgument(MoveFileActionResultsArgs.FailedFilesPaths, failedFilesPaths)
                );
            }
        }
    }
}