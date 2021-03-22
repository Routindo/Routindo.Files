using Routindo.Contract;
using Routindo.Contract.Attributes;
using Routindo.Plugins.Files.Components.Actions.DeleteFIlesByPattern;
using Routindo.Plugins.Files.Components.Actions.MoveFiles;
using Routindo.Plugins.Files.Components.Watchers;
using Routindo.Plugins.Files.UI.Views;

[assembly: ComponentConfigurator(typeof(FilesWatcherConfigurator), FilesWatcher.ComponentUniqueId, "Configurator for Files Watcher")]
[assembly: ComponentConfigurator(typeof(MoveFileActionConfigurator), MoveFileAction.ComponentUniqueId, "Configurator for File Mover action")]
[assembly: ComponentConfigurator(typeof(DeleteByPatternView), DeleteFilesByPatternFromDirectory.ComponentUniqueId, "Configurator for Delete files by pattern")]