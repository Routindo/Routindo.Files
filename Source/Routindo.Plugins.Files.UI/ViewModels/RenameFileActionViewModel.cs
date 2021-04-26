using System.Windows.Input;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.Files.Components.Actions.Rename;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Routindo.Plugins.Files.UI.ViewModels
{
    public class RenameFileActionViewModel: PluginConfiguratorViewModelBase
    {
        private string _destinationExtension;
        private string _destinationPrefix;
        private string _destinationFileName;
        private string _sourceFilePath;
        private bool _isCheckedPrefixAndExtensionDestinationFileName;
        private bool _isCheckedNewDestinationFileName = true;

        public RenameFileActionViewModel() 
        { 
            SelectSourceFilePathCommand = new RelayCommand(SelectSourceFilePath);
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

        public ICommand SelectSourceFilePathCommand { get; }

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
                ValidatePrefixAndExtension();
                OnPropertyChanged();
                //OnPropertyChanged(nameof(IsCheckedPrefixAndExtensionDestinationFileName));
                //OnPropertyChanged(nameof(IsCheckedNewDestinationFileName));
            }
        }

        private void ValidatePrefixAndExtension()
        {
            ClearPropertyErrors(nameof(DestinationPrefix));
            ClearPropertyErrors(nameof(DestinationExtension));
            if (IsCheckedPrefixAndExtensionDestinationFileName &&
                string.IsNullOrWhiteSpace(DestinationExtension) && string.IsNullOrWhiteSpace(DestinationPrefix))
            {
                AddPropertyError(nameof(DestinationPrefix), "This field is mandatory");
                AddPropertyError(nameof(DestinationExtension), "This field is mandatory");
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
                ValidatePrefixAndExtension();
                OnPropertyChanged();
                //OnPropertyChanged(nameof(IsCheckedPrefixAndExtensionDestinationFileName));
                //OnPropertyChanged(nameof(IsCheckedNewDestinationFileName));
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
                ValidateNewFileName();
                OnPropertyChanged();
                //OnPropertyChanged(nameof(IsCheckedPrefixAndExtensionDestinationFileName));
                //OnPropertyChanged(nameof(IsCheckedNewDestinationFileName));
            }
        }

        private void ValidateNewFileName()
        {
            ClearPropertyErrors(nameof(DestinationFileName));
            if (IsCheckedNewDestinationFileName && string.IsNullOrWhiteSpace(DestinationFileName))
            {
                ValidateNonNullOrEmptyString(DestinationFileName, nameof(DestinationFileName));
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
                .WithArgument(RenameFileActionArgs.DestinationExtension, DestinationExtension)
                .WithArgument(RenameFileActionArgs.DestinationPrefix, DestinationPrefix)
                .WithArgument(RenameFileActionArgs.SourceFilePath, SourceFilePath)
                .WithArgument(RenameFileActionArgs.DestinationFileName, DestinationFileName);
        }


        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null)
                return;

            if (arguments.HasArgument(RenameFileActionArgs.DestinationExtension))
            {
                DestinationExtension = arguments.GetValue<string>(RenameFileActionArgs.DestinationExtension);
            }

            if (arguments.HasArgument(RenameFileActionArgs.DestinationPrefix))
            {
                DestinationPrefix = arguments.GetValue<string>(RenameFileActionArgs.DestinationPrefix);
            }

            IsCheckedPrefixAndExtensionDestinationFileName = !string.IsNullOrWhiteSpace(DestinationPrefix) ||
                                                             !string.IsNullOrWhiteSpace(DestinationExtension);

            if (arguments.HasArgument(RenameFileActionArgs.SourceFilePath))
            {
                SourceFilePath = arguments.GetValue<string>(RenameFileActionArgs.SourceFilePath);
            }

            if (arguments.HasArgument(RenameFileActionArgs.DestinationFileName))
            {
                DestinationFileName = arguments.GetValue<string>(RenameFileActionArgs.DestinationFileName);
                IsCheckedNewDestinationFileName = !IsCheckedPrefixAndExtensionDestinationFileName;
            }
        }

        public bool IsCheckedNewDestinationFileName
        {
            get => _isCheckedNewDestinationFileName;
            set
            {
                _isCheckedNewDestinationFileName = value;
                ValidateNewFileName();
                OnPropertyChanged();
            }
        }

        public bool IsCheckedPrefixAndExtensionDestinationFileName
        {
            get => _isCheckedPrefixAndExtensionDestinationFileName;
            set
            {
                _isCheckedPrefixAndExtensionDestinationFileName = value;
                ValidatePrefixAndExtension();
                OnPropertyChanged();
            }
        }

        protected override void ValidateProperties()
        {
            base.ValidateProperties();
            ValidateNewFileName();
            ValidatePrefixAndExtension();
        }
    }
}
