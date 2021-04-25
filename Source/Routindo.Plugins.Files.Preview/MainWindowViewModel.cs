using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.Files.Components;
using Routindo.Plugins.Files.Preview.Annotations;
using Routindo.Plugins.Files.UI.ViewModels;

namespace Routindo.Plugins.Files.Preview
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            FilesWatcherConfiguratorViewModel = new FilesWatcherConfiguratorViewModel();
            FilesWatcherConfiguratorViewModel.SetArguments(ArgumentCollection.New()
                .WithArgument(FilesSelectorArgs.CreatedAfter, TimeSpan.FromHours(1).TotalMilliseconds)
                .WithArgument(FilesSelectorArgs.CreatedBefore, TimeSpan.FromMinutes(1).TotalMilliseconds)
                .WithArgument(FilesSelectorArgs.EditedAfter, TimeSpan.FromMinutes(45).TotalMilliseconds)
                .WithArgument(FilesSelectorArgs.EditedBefore, TimeSpan.FromMinutes(15).TotalMilliseconds)
                .WithArgument(FilesSelectorArgs.SortingCriteria, FilesSelectionSortingCriteria.CreationTimeDescending)
                .WithArgument(FilesSelectorArgs.Pattern, "*.txt")
            );
            ConfigureCommand = new RelayCommand(() => FilesWatcherConfiguratorViewModel.Configure(),
                () => this.FilesWatcherConfiguratorViewModel.CanConfigure());
            SetArgumentCommand = new RelayCommand(() =>
                this.FilesWatcherConfiguratorViewModel.SetArguments(ArgumentCollection.New()));
            // FilesWatcherConfiguratorViewModel.PropertyChanged += FilesWatcherConfiguratorViewModelOnPropertyChanged;
        }

        //private void FilesWatcherConfiguratorViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    ((RelayCommand)this.ConfigureCommand).
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        public FilesSelectorViewModel FilesWatcherConfiguratorViewModel { get; set; }



        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand ConfigureCommand { get; set; }
        public ICommand SetArgumentCommand { get; set; }
    }
}
