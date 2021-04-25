using Routindo.Plugins.Files.UI.ViewModels;

namespace Routindo.Plugins.Files.UI.Views
{
    public class FilesWatcherConfigurator: FilesSelectorConfigurator
    {
        public FilesWatcherConfigurator()
        {
            this.DataContext = new FilesWatcherConfiguratorViewModel();
        }
    }
}
