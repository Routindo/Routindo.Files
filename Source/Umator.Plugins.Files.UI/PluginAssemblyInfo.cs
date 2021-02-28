using Umator.Contract;
using Umator.Plugins.Files.Components.Actions.DeleteFIlesByPattern;
using Umator.Plugins.Files.Components.Actions.MoveFiles;
using Umator.Plugins.Files.Components.Watchers;
using Umator.Plugins.Files.UI.Views;

[assembly: ComponentConfigurator(typeof(FilesWatcherConfigurator), FilesWatcher.ComponentUniqueId, "Configurator for Files Watcher")]
[assembly: ComponentConfigurator(typeof(MoveFileActionConfigurator), MoveFileAction.ComponentUniqueId, "Configurator for File Mover action")]
[assembly: ComponentConfigurator(typeof(DeleteByPatternView), DeleteFilesByPatternFromDirectory.ComponentUniqueId, "Configurator for Delete files by pattern")]