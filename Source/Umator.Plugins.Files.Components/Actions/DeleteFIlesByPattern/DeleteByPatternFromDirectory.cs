using System;
using System.IO;
using System.Linq;
using Umator.Contract;
using Umator.Contract.Services;

namespace Umator.Plugins.Files.Components.Actions.DeleteFIlesByPattern
{
    [PluginItemInfo(ComponentUniqueId, nameof(DeleteFilesByPatternFromDirectory),
        "Delete files using a specific pattern from a specific directory")]
    public class DeleteFilesByPatternFromDirectory : IAction
    {
        public const string ComponentUniqueId = "B2FF2D73-C1F7-45B2-B39D-D392837A4FA2";

        [Argument(DeleteFilesByPatternFromDirectoryArgs.Directory, true)]
        public string DirectoryPath { get; set; }

        [Argument(DeleteFilesByPatternFromDirectoryArgs.Pattern, true)]
        public string Pattern { get; set; }

        [Argument(DeleteFilesByPatternFromDirectoryArgs.MaximumFiles, true)]
        public int MaximumFiles { get; set; }

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

                var files = Directory.GetFiles(DirectoryPath, Pattern, SearchOption.TopDirectoryOnly).Take(this.MaximumFiles > 0 ? int.MaxValue : this.MaximumFiles).ToList();

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
