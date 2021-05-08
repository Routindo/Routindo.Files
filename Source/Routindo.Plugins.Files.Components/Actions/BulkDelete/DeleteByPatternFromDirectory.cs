using System;
using System.Collections.Generic;
using System.IO;
using Routindo.Contract.Actions;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;

namespace Routindo.Plugins.Files.Components.Actions.BulkDelete
{
    [PluginItemInfo(ComponentUniqueId, nameof(DeleteFilesByPatternFromDirectory),
        "Delete files using a specific pattern from a specific directory", Category = "Files", FriendlyName = "Bulk Delete Files")]
    [ResultArgumentsClass(typeof(DeleteFilesByPatternFromDirectoryResultsArgs))]
    public class DeleteFilesByPatternFromDirectory : FilesSelector, IAction
    {
        public const string ComponentUniqueId = "B2FF2D73-C1F7-45B2-B39D-D392837A4FA2";

        protected override ILoggingService Logger => this.LoggingService;

        public string Id { get; set; }

        public ILoggingService LoggingService { get; set; }

        public ActionResult Execute(ArgumentCollection arguments)
        {
            List<string> deletedFiles = new List<string>();
            List<string> failedFiles = new List<string>();
            try
            {
                if (!Directory.Exists(DirectoryPath))
                {
                    LoggingService.Error($"Directory doesn't exist [{DirectoryPath}]");
                    return ActionResult.Failed();
                }

                var files = this.Select();

                foreach (var file in files)
                {
                    try
                    {
                        LoggingService.Debug($"Deleting ({file})");
                        File.Delete(file);
                        LoggingService.Info($"({file}) deleted successfully");
                        deletedFiles.Add(file);
                    }
                    catch (Exception exception)
                    {
                        LoggingService.Error($"Deletion failed of ({file})");
                        LoggingService.Error(exception);
                        failedFiles.Add(file);
                        continue;
                    }
                }
                return ActionResult.Succeeded().WithAdditionInformation(ArgumentCollection.New()
                    .WithArgument(DeleteFilesByPatternFromDirectoryResultsArgs.DeletedFiles, deletedFiles)
                    .WithArgument(DeleteFilesByPatternFromDirectoryResultsArgs.FailedFiles, failedFiles)
                );
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return ActionResult.Failed(exception).WithAdditionInformation(ArgumentCollection.New()
                    .WithArgument(DeleteFilesByPatternFromDirectoryResultsArgs.DeletedFiles, deletedFiles)
                    .WithArgument(DeleteFilesByPatternFromDirectoryResultsArgs.FailedFiles, failedFiles)
                );
            }
        }
    }
}
