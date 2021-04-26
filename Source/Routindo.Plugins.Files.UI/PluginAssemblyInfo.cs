using Routindo.Contract.Attributes;
using Routindo.Plugins.Files.Components.Actions.BulkDelete;
using Routindo.Plugins.Files.Components.Actions.Copy;
using Routindo.Plugins.Files.Components.Actions.Move;
using Routindo.Plugins.Files.Components.Watchers;
using Routindo.Plugins.Files.UI.Views;

[assembly: ComponentConfigurator(typeof(FilesWatcherConfigurator), FilesWatcher.ComponentUniqueId, "Configurator for Files Watcher")]
[assembly: ComponentConfigurator(typeof(MoveFileActionConfigurator), MoveFileAction.ComponentUniqueId, "Configurator for File Mover action")]
[assembly: ComponentConfigurator(typeof(DeleteByPatternView), DeleteFilesByPatternFromDirectory.ComponentUniqueId, "Configurator for Delete files by pattern")]
[assembly: ComponentConfigurator(typeof(CopyFilesActionView), CopyFilesAction.ComponentUniqueId, "Configurator for File Copier action")]