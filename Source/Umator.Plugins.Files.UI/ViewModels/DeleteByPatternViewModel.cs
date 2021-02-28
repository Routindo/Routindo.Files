using System.Linq;
using Umator.Contract;
using Umator.Contract.UI;
using Umator.Plugins.Files.Components.Actions.DeleteFIlesByPattern;

namespace Umator.Plugins.Files.UI.ViewModels
{
    public class DeleteByPatternViewModel : PluginConfiguratorViewModelBase
    {
        private string _directory;
        private string _pattern;
        private int _maximumFiles;

        public string Directory
        {
            get => _directory;
            set
            {
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
                _maximumFiles = value;
                ValidateNumber(MaximumFiles, i => i> 0);
                OnPropertyChanged();
            }
        }

        protected override void ValidateProperties()
        {
            // Directory
            ValidateNonNullOrEmptyString(Directory);
            OnPropertyChanged();

            // Maximum Files
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
                Directory = arguments[DeleteFilesByPatternFromDirectoryArgs.Directory].ToString();

            if (arguments.HasArgument(DeleteFilesByPatternFromDirectoryArgs.Pattern))
                Pattern = arguments[DeleteFilesByPatternFromDirectoryArgs.Pattern].ToString();

            if (arguments.HasArgument(DeleteFilesByPatternFromDirectoryArgs.MaximumFiles))
            {
                if (int.TryParse(arguments[DeleteFilesByPatternFromDirectoryArgs.MaximumFiles].ToString(), out int maximumFiles))
                {
                    MaximumFiles = maximumFiles;
                }
            }
        }
    }
}
