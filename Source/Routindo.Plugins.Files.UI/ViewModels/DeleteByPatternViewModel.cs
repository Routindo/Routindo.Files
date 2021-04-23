using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Routindo.Contract;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.Files.Components.Actions.DeleteFIlesByPattern;

namespace Routindo.Plugins.Files.UI.ViewModels
{
    public class DeleteByPatternViewModel : PluginConfiguratorViewModelBase
    {
        private string _directory;
        private string _pattern;
        private int _maximumFiles;

        public DeleteByPatternViewModel()
        {
            this.SelectDirectoryCommand = new ActionCommand(SelectDirectory);
        }

        public ICommand SelectDirectoryCommand { get; }

        public string Directory
        {
            get => _directory;
            set
            {
                ClearPropertyErrors();
                _directory = value;
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
                ClearPropertyErrors();
                _maximumFiles = value;
                ValidateNumber(MaximumFiles, i => i> 0);
                OnPropertyChanged();
            }
        }

        protected override void ValidateProperties()
        {
            // Directory
            ClearPropertyErrors(nameof(Directory));
            ValidateNonNullOrEmptyString(Directory);
            OnPropertyChanged();

            // Maximum Files
            ClearPropertyErrors(nameof(MaximumFiles));
            ValidateNumber(MaximumFiles, i => i > 0);
            OnPropertyChanged();
        }

        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(DeleteFilesByPatternFromDirectoryArgs.Directory, Directory)
                .WithArgument(DeleteFilesByPatternFromDirectoryArgs.Pattern, Pattern)
                .WithArgument(DeleteFilesByPatternFromDirectoryArgs.MaximumFiles, MaximumFiles);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(DeleteFilesByPatternFromDirectoryArgs.Directory))
                Directory = arguments.GetValue<string>(DeleteFilesByPatternFromDirectoryArgs.Directory);

            if (arguments.HasArgument(DeleteFilesByPatternFromDirectoryArgs.Pattern))
                Pattern = arguments.GetValue<string>(DeleteFilesByPatternFromDirectoryArgs.Pattern);

            if (arguments.HasArgument(DeleteFilesByPatternFromDirectoryArgs.MaximumFiles))
                MaximumFiles =arguments.GetValue<int>(DeleteFilesByPatternFromDirectoryArgs.MaximumFiles);
            
        }

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
    }
}
