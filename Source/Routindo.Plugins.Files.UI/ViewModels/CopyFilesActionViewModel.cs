using System.Windows.Forms;
using System.Windows.Input;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.Files.Components.Actions.Copy;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Routindo.Plugins.Files.UI.ViewModels
{
    public class CopyFilesActionViewModel: PluginConfiguratorViewModelBase
    {
        private string _destinationDirectory;
        private string _sourceFilePath;

        public CopyFilesActionViewModel()
        {
            SelectDestinationDirectoryCommand = new RelayCommand(SelectDestinationDirectory);
            SelectSourceFilePathCommand = new RelayCommand(SelectSourceFilePath);
        }

        public ICommand SelectDestinationDirectoryCommand { get; }
        public ICommand SelectSourceFilePathCommand { get; }

        private void SelectDestinationDirectory()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(DestinationDirectory))
                {
                    dialog.SelectedPath = DestinationDirectory;
                }

                dialog.Description = "Directory where to copy the files";
                dialog.ShowNewFolderButton = true;
                dialog.UseDescriptionForTitle = true;
                var dialogResult = dialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    DestinationDirectory = dialog.SelectedPath;
                }
            }
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
                .WithArgument(CopyFilesActionArgs.DestinationDirectory, DestinationDirectory)
                .WithArgument(CopyFilesActionArgs.SourceFilePath, SourceFilePath);
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if (arguments == null)
                return;

            if (arguments.HasArgument(CopyFilesActionArgs.DestinationDirectory))
            {
                DestinationDirectory = arguments.GetValue<string>(CopyFilesActionArgs.DestinationDirectory);
            }

            if (arguments.HasArgument(CopyFilesActionArgs.SourceFilePath))
            {
                SourceFilePath = arguments.GetValue<string>(CopyFilesActionArgs.SourceFilePath);
            }
        }
    }
}
