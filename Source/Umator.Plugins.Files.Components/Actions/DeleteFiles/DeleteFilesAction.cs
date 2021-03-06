using System;
using System.Collections.Generic;
using System.IO;
using Umator.Contract;
using Umator.Contract.Services;

namespace Umator.Plugins.Files.Components.Actions.DeleteFiles
{
    [PluginItemInfo("6103E4DD-4D75-4C86-AD61-2F02E802D15E", "Files Deleter", "Delete specific files")]
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