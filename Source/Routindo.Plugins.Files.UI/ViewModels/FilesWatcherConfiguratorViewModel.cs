using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Routindo.Contract;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.Files.Components.Watchers;

namespace Routindo.Plugins.Files.UI.ViewModels
{
    public class FilesWatcherConfiguratorViewModel: PluginConfiguratorViewModelBase
    {
        private string _directory;
        private string _pattern;
        private int _maximumFiles;

        public FilesWatcherConfiguratorViewModel()
        {
            this.SelectDirectoryCommand = new ActionCommand(SelectDirectory);
        }

        public string Directory
        {
            get => _directory;
            set
            {
                _directory = value;
                ClearPropertyErrors();
                ValidateNonNullOrEmptyString(Directory);
                OnPropertyChanged();
            }
        }

        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                OnPropertyChanged();
            }
        }

        public int MaximumFiles
        {
            get => _maximumFiles;
            set
            {
                _maximumFiles = value;
                ClearPropertyErrors();
                ValidateNumber(MaximumFiles, i => i> 0);
                OnPropertyChanged();
            }
        }

        public ICommand SelectDirectoryCommand { get; }

        private void SelectDirectory()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(Directory))
                {
                    dialog.SelectedPath = Directory;
                }

                dialog.Description = "Directory where to watch for new files";
                dialog.ShowNewFolderButton = true;
                dialog.UseDescriptionForTitle = true;
                var dialogResult = dialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    Directory = dialog.SelectedPath;
                }
            }
        }


        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(FilesWatcherArguments.Directory, Directory)
                .WithArgument(FilesWatcherArguments.Pattern, Pattern)
                .WithArgument(FilesWatcherArguments.MaximumFiles, MaximumFiles);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if(arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(FilesWatcherArguments.Directory))
                Directory = arguments[FilesWatcherArguments.Directory].ToString();

            if (arguments.HasArgument(FilesWatcherArguments.Pattern))
                Pattern = arguments[FilesWatcherArguments.Pattern].ToString();

            if (arguments.HasArgument(FilesWatcherArguments.MaximumFiles))
            {
                if (int.TryParse(arguments[FilesWatcherArguments.MaximumFiles].ToString(), out int maximumFiles))
                {
                    MaximumFiles = maximumFiles;
                }
            }
        }

        protected override void ValidateProperties()
        {
            // Directory
            ClearPropertyErrors(nameof(Directory));
            ValidateNonNullOrEmptyString(Directory, nameof(Directory));
            OnPropertyChanged(nameof(Directory));

            // Maximum files
            ClearPropertyErrors(nameof(Directory));
            ValidateNumber(MaximumFiles, i => i > 0, nameof(MaximumFiles));
            OnPropertyChanged(nameof(MaximumFiles));
        }
    }
}
