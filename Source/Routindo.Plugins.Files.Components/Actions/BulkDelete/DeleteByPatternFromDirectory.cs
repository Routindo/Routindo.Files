using System;
using System.IO;
using Routindo.Contract.Actions;
using Routindo.Contract.Arguments;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;

namespace Routindo.Plugins.Files.Components.Actions.BulkDelete
{
    [PluginItemInfo(ComponentUniqueId, nameof(DeleteFilesByPatternFromDirectory),
        "Delete files using a specific pattern from a specific directory")]
    public class DeleteFilesByPatternFromDirectory : FilesSelector, IAction
    {
        public const string ComponentUniqueId = "B2FF2D73-C1F7-45B2-B39D-D392837A4FA2";

        protected override ILoggingService Logger => this.LoggingService;

        public string Id { get; set; }

        public ILoggingService LoggingService { get; set; }

        public ActionResult Execute(ArgumentCollection arguments)
        {
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
                    }
                    catch (Exception exception)
                    {
                        LoggingService.Error($"Deletion failed of ({file})");
                        LoggingService.Error(exception);
                        continue;
                    }
                }
                return ActionResult.Succeeded();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                return ActionResult.Failed();
            }
        }

    }
}
