﻿using System;
using System.Collections.Generic;
using System.IO;
using Routindo.Contract.Actions;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Exceptions;
using Routindo.Contract.Services;

namespace Routindo.Plugins.Files.Components.Actions.Delete
{
    [PluginItemInfo("6103E4DD-4D75-4C86-AD61-2F02E802D15E", nameof(DeleteFilesAction), 
        "Delete one or more files using their paths returned by the Watcher", Category = "Files", FriendlyName = "Delete Files")]
    [ExecutionArgumentsClass(typeof(DeleteFilesActionExecutionArgs))]
    public class DeleteFilesAction :
        IAction
    {

        public string Id { get; set; }
        public ILoggingService LoggingService { get; set; }

        public ActionResult Execute(ArgumentCollection arguments)
        {
            try
            {
                // Must provide arguments
                if (arguments == null)
                    throw new ArgumentNullException(nameof(arguments));

                if (!arguments.HasArgument(DeleteFilesActionExecutionArgs.FilesPaths))
                    throw new MissingArgumentException(DeleteFilesActionExecutionArgs.FilesPaths);

                if (!(arguments[DeleteFilesActionExecutionArgs.FilesPaths] is List<string> filePaths))
                    throw new ArgumentsValidationException(
                        $"unable to cast argument value into list of string. key({DeleteFilesActionExecutionArgs.FilesPaths})");

                foreach (var filePath in filePaths)
                {
                    // File must exist
                    if (!File.Exists(filePath))
                        throw new FileNotFoundException("File not found", filePath);

                    File.Delete(filePath);
                    LoggingService.Info($"File ({filePath}) deleted successfully");
                }

                return new ActionResult(true);
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

        public bool ValidateArguments(ArgumentCollection arguments)
        {
            if (!arguments.HasArgument(DeleteFilesActionExecutionArgs.FilesPaths))
            {
                LoggingService.Error($"Missing mandatory argument {DeleteFilesActionExecutionArgs.FilesPaths}");
                return false;
            }

            return true;
        }
    }
}