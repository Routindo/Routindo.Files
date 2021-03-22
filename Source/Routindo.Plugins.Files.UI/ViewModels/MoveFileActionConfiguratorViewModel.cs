using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Routindo.Contract;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.Files.Components.Actions.MoveFiles;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Routindo.Plugins.Files.UI.ViewModels
{
    public class MoveFileActionConfiguratorViewModel: PluginConfiguratorViewModelBase
    {
        private string _destinationDirectory;
        private string _destinationExtension;
        private string _destinationPrefix;
        private string _destinationFileName;
        private string _sourceFilePath;

        public MoveFileActionConfiguratorViewModel()
        {
            SelectDestinationDirectoryCommand = new ActionCommand(SelectDestinationDirectory);
            SelectSourceFilePathCommand = new ActionCommand(SelectSourceFilePath);
            ResetDestinationFileNameCommand = new ActionCommand(ResetDestinationFileName);
        }

        private void ResetDestinationFileName()
        {
            this.DestinationFileName = string.Empty;
            this.DestinationExtension = string.Empty;
            this.DestinationPrefix = string.Empty;
        }

        private void SelectSourceFilePath()
        {
            var openFileDialog = new OpenFileDialog { CheckFileExists = false, Title = "Select File" };
            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                this.SourceFilePath = openFileDialog.FileName;
            }
        }

        public ICommand SelectDestinationDirectoryCommand { get; }
        public ICommand SelectSourceFilePathCommand { get; } 
        public ICommand ResetDestinationFileNameCommand { get; } 

        private void SelectDestinationDirectory()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(DestinationDirectory))
                {
                    dialog.SelectedPath = DestinationDirectory;
                }

                dialog.Description = "Directory where to watch for new files";
                dialog.ShowNewFolderButton = true;
                dialog.UseDescriptionForTitle = true;
                var dialogResult = dialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    DestinationDirectory = dialog.SelectedPath;
                }
            }
        }

        public string DestinationDirectory
        {
            get => _destinationDirectory;
            set
            {
                _destinationDirectory = value;
                ClearPropertyErrors();
                ValidateNonNullOrEmptyString(DestinationDirectory);
                OnPropertyChanged();
            }
        }

        public string DestinationExtension
        {
            get => _destinationExtension;
            set
            {
                _destinationExtension = value;
                if (!string.IsNullOrWhiteSpace(_destinationExtension) && !string.IsNullOrWhiteSpace(DestinationFileName))
                { 
                    this.DestinationFileName = string.Empty;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckedPrefixAndExtensionDestinationFileName));
                OnPropertyChanged(nameof(IsCheckedNoneDestinationFileName));
                OnPropertyChanged(nameof(IsCheckedNewDestinationFileName));
            }
        }

        public string DestinationPrefix
        {
            get => _destinationPrefix;
            set
            {
                _destinationPrefix = value;
                if (!string.IsNullOrWhiteSpace(_destinationPrefix) && !string.IsNullOrWhiteSpace(DestinationFileName))
                {
                    this.DestinationFileName = string.Empty;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckedPrefixAndExtensionDestinationFileName));
                OnPropertyChanged(nameof(IsCheckedNoneDestinationFileName));
                OnPropertyChanged(nameof(IsCheckedNewDestinationFileName));
            }
        }

        public string DestinationFileName
        {
            get => _destinationFileName;
            set
            {
                _destinationFileName = value;
                if (!string.IsNullOrWhiteSpace(_destinationFileName))
                {
                    if (!string.IsNullOrWhiteSpace(DestinationPrefix))
                        this.DestinationPrefix = string.Empty;
                    if (!string.IsNullOrWhiteSpace(DestinationExtension))
                        this.DestinationExtension = string.Empty;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCheckedPrefixAndExtensionDestinationFileName));
                OnPropertyChanged(nameof(IsCheckedNoneDestinationFileName));
                OnPropertyChanged(nameof(IsCheckedNewDestinationFileName));
            }
        }

        public string SourceFilePath
        {
            get => _sourceFilePath;
            set
            {
                _sourceFilePath = value;
                OnPropertyChanged();
            }
        }

        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(MoveFileActionInstanceArgs.DestinationDirectory, DestinationDirectory)
                .WithArgument(MoveFileActionInstanceArgs.DestinationExtension, DestinationExtension)
                .WithArgument(MoveFileActionInstanceArgs.DestinationPrefix, DestinationPrefix)
                .WithArgument(MoveFileActionInstanceArgs.SourceFilePath, SourceFilePath)
                .WithArgument(MoveFileActionInstanceArgs.DestinationFileName, DestinationFileName);
        }


        public override void SetArguments(ArgumentCollection arguments)
        {
            if(arguments == null)
                return;

            if (arguments.HasArgument(MoveFileActionInstanceArgs.DestinationDirectory))
            {
                DestinationDirectory = arguments.GetValue<string>(MoveFileActionInstanceArgs.DestinationDirectory);
            }

            if (arguments.HasArgument(MoveFileActionInstanceArgs.DestinationExtension))
            {
                DestinationExtension = arguments.GetValue<string>(MoveFileActionInstanceArgs.DestinationExtension);
            }

            if (arguments.HasArgument(MoveFileActionInstanceArgs.DestinationPrefix))
            {
                DestinationPrefix = arguments.GetValue<string>(MoveFileActionInstanceArgs.DestinationPrefix);
            }

            if (arguments.HasArgument(MoveFileActionInstanceArgs.SourceFilePath))
            {
                SourceFilePath = arguments.GetValue<string>(MoveFileActionInstanceArgs.SourceFilePath);
            }

            if (arguments.HasArgument(MoveFileActionInstanceArgs.DestinationFileName))
            {
                DestinationFileName = arguments.GetValue<string>(MoveFileActionInstanceArgs.DestinationFileName);
            }
        }

        protected override void ValidateProperties()
        {
            ValidateNonNullOrEmptyString(DestinationDirectory, nameof(DestinationDirectory));
            OnPropertyChanged(nameof(DestinationDirectory));
        }

        public bool IsCheckedNoneDestinationFileName
        {
            get { return !IsCheckedNewDestinationFileName && !IsCheckedPrefixAndExtensionDestinationFileName; }
        }

        public bool IsCheckedNewDestinationFileName
        {
            get { return !string.IsNullOrWhiteSpace(this.DestinationFileName); }
        }

        public bool IsCheckedPrefixAndExtensionDestinationFileName
        {
            get
            {
                return !string.IsNullOrWhiteSpace(DestinationPrefix) ||
                       !string.IsNullOrWhiteSpace(DestinationExtension);
            }
        }
    }
}
