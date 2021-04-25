using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using Routindo.Contract.Arguments;
using Routindo.Contract.UI;
using Routindo.Plugins.Files.Components;
using Routindo.Plugins.Files.UI.Enums;

namespace Routindo.Plugins.Files.UI.ViewModels
{
    public abstract class FilesSelectorViewModel: PluginConfiguratorViewModelBase
    {
        private string _directory;
        private string _pattern;
        private int _maximumFiles = 1;
        private int _createdBefore = 1;
        private int _createdAfter = 1;
        private int _editedBefore =1;
        private int _editedAfter =1;
        private TimePeriod _createdBeforePeriod;
        private TimePeriod _createdAfterPeriod;
        private TimePeriod _editedBeforePeriod;
        private TimePeriod _editedAfterPeriod;
        private bool _filterByCreatedBefore;
        private bool _filterByCreatedAfter;
        private bool _filterByEditedBefore;
        private bool _filterByEditedAfter;
        private FilesSelectionSortingCriteria _sortingCriteria;
        private bool _filterByCreationTime;
        private bool _filterByEditionTime;

        public FilesSelectorViewModel()
        {
            this.SelectDirectoryCommand = new RelayCommand(SelectDirectory);
            TimePeriods = new ObservableCollection<TimePeriod>(Enum.GetValues<TimePeriod>());
            SortingCriterias = new ObservableCollection<FilesSelectionSortingCriteria>(Enum.GetValues<FilesSelectionSortingCriteria>());
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

        public ObservableCollection<TimePeriod> TimePeriods { get; }

        #region sorting
        public ObservableCollection<FilesSelectionSortingCriteria> SortingCriterias { get; }

        public FilesSelectionSortingCriteria SortingCriteria
        {
            get => _sortingCriteria;
            set
            {
                _sortingCriteria = value;
                OnPropertyChanged();
            }
        }
        #endregion 

        #region Creation time
        public bool FilterByCreationTime
        {
            get => _filterByCreationTime;
            set
            {
                _filterByCreationTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByCreationTimeHasError));
                if (!value)
                {
                    if (FilterByCreatedAfter)
                        FilterByCreatedAfter = false;

                    if (FilterByCreatedBefore)
                        FilterByCreatedBefore = false;
                }
            }
        }

        public bool FilterByCreationTimeHasError
        {
            get
            {
                ClearPropertyErrors();
                var result = FilterByCreationTime &&
                             this.FilterByCreatedBefore
                             && FilterByCreatedAfter
                             && GetTimeSpanFromFilter(this.CreatedAfterPeriod, this.CreatedAfter).TotalMilliseconds >=
                             GetTimeSpanFromFilter(this.CreatedBeforePeriod, this.CreatedBefore).TotalMilliseconds;
                if (result)
                {
                    AddPropertyError(nameof(FilterByCreationTimeHasError), "Creation Time filter has errors");
                }
                return result;
            }
        }

        public bool FilterByCreatedBefore
        {
            get => _filterByCreatedBefore;
            set
            {
                _filterByCreatedBefore = value;
                OnPropertyChanged();
                FilterByCreationTime = _filterByCreatedBefore || FilterByCreatedAfter;
                OnPropertyChanged(nameof(FilterByCreationTimeHasError));
            }
        }

        public bool FilterByCreatedAfter
        {
            get => _filterByCreatedAfter;
            set
            {
                _filterByCreatedAfter = value;
                OnPropertyChanged();
                FilterByCreationTime = _filterByCreatedAfter || FilterByCreatedBefore;
                OnPropertyChanged(nameof(FilterByCreationTimeHasError));
            }
        }


        public TimePeriod CreatedBeforePeriod
        {
            get => _createdBeforePeriod;
            set
            {
                _createdBeforePeriod = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByCreationTimeHasError));
            }
        }

        public TimePeriod CreatedAfterPeriod
        {
            get => _createdAfterPeriod;
            set
            {
                _createdAfterPeriod = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByCreationTimeHasError));
            }
        }

        public int CreatedBefore
        {
            get => _createdBefore;
            set
            {
                _createdBefore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByCreationTimeHasError));
            }
        }

        public int CreatedAfter
        {
            get => _createdAfter;
            set
            {
                _createdAfter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByCreationTimeHasError));
            }
        }
        #endregion

        #region Edition time

        public bool FilterByEditionTime
        {
            get => _filterByEditionTime;
            set
            {
                _filterByEditionTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByEditionTimeHasError));

                if (!value)
                {
                    if (FilterByEditedAfter)
                        FilterByEditedAfter = false;

                    if (FilterByEditedBefore)
                        FilterByEditedBefore = false;
                }
            }
        }

        public bool FilterByEditionTimeHasError
        {
            get
            {
                ClearPropertyErrors();
                var result = FilterByEditionTime &&
                             this.FilterByEditedBefore
                             && FilterByEditedAfter
                             && GetTimeSpanFromFilter(this.EditedAfterPeriod, this.EditedAfter).TotalMilliseconds >=
                             GetTimeSpanFromFilter(this.EditedBeforePeriod, this.EditedBefore).TotalMilliseconds;

                if(result)
                    AddPropertyError(nameof(FilterByEditionTimeHasError), "Edition Time filter has errors");

                return result;
            }
        }

        public TimePeriod EditedBeforePeriod
        {
            get => _editedBeforePeriod;
            set
            {
                _editedBeforePeriod = value;
                OnPropertyChanged();
                FilterByEditionTime = _filterByEditedBefore || FilterByEditedAfter;
                OnPropertyChanged(nameof(FilterByEditionTimeHasError));
            }
        }

        public TimePeriod EditedAfterPeriod
        {
            get => _editedAfterPeriod;
            set
            {
                _editedAfterPeriod = value;
                OnPropertyChanged();
                FilterByEditionTime = _filterByEditedAfter || FilterByEditedBefore;
                OnPropertyChanged(nameof(FilterByEditionTimeHasError));
            }
        }

        public bool FilterByEditedBefore
        {
            get => _filterByEditedBefore;
            set
            {
                _filterByEditedBefore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByEditionTime));
                OnPropertyChanged(nameof(FilterByEditionTimeHasError));
            }
        }

        public bool FilterByEditedAfter
        {
            get => _filterByEditedAfter;
            set
            {
                _filterByEditedAfter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByEditionTime));
                OnPropertyChanged(nameof(FilterByEditionTimeHasError));
            }
        }

        public int EditedBefore
        {
            get => _editedBefore;
            set
            {
                _editedBefore = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByEditionTimeHasError));
            }
        }

        public int EditedAfter
        {
            get => _editedAfter;
            set
            {
                _editedAfter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterByEditionTimeHasError));
            }
        }

        #endregion 

        

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

        private TimeSpan GetTimeSpanFromFilter(TimePeriod timePeriod, int timeValue)
        {
            TimeSpan timeSpan;
            switch (timePeriod)
            {
                case TimePeriod.Days:
                {
                    timeSpan = TimeSpan.FromDays(timeValue);
                    break;
                }
                case TimePeriod.Hours:
                {
                    timeSpan = TimeSpan.FromHours(timeValue);
                    break;
                }
                case TimePeriod.Minutes:
                {
                    timeSpan = TimeSpan.FromMinutes(timeValue);
                    break;
                }
                case TimePeriod.Seconds:
                {
                    timeSpan = TimeSpan.FromSeconds(timeValue);
                    break;
                }
                default:
                {
                    timeSpan = TimeSpan.FromMilliseconds(timeValue);
                    break;
                }
            }

            return timeSpan;
        }

        private bool TryGetTimePeriodFromMilliseconds(ulong? milliseconds, out TimePeriod timePeriod, out int timeValue)
        {
            if (milliseconds.HasValue)
            {
                var ts = TimeSpan.FromMilliseconds(milliseconds.Value);
                if (ts.Days > 0)
                {
                    timePeriod = TimePeriod.Days;
                    timeValue = ts.Days;
                }
                else if (ts.Hours > 0)
                {
                    timePeriod = TimePeriod.Hours;
                    timeValue = ts.Hours;
                }
                else if (ts.Minutes > 0)
                {
                    timePeriod = TimePeriod.Minutes;
                    timeValue = ts.Minutes;
                }
                else if (ts.Seconds > 0)
                {
                    timePeriod = TimePeriod.Seconds;
                    timeValue = ts.Seconds;
                }
                else
                {
                    timePeriod = TimePeriod.Milliseconds;
                    timeValue = ts.Milliseconds;
                }

                return true;
            }

            timePeriod = TimePeriod.Milliseconds;
            timeValue = 0;
            return false;
        }

        public override void Configure()
        {
            this.InstanceArguments = ArgumentCollection.New()
                .WithArgument(FilesSelectorArgs.Directory, Directory)
                .WithArgument(FilesSelectorArgs.Pattern, Pattern)
                .WithArgument(FilesSelectorArgs.MaximumFiles, MaximumFiles)
                .WithArgument(FilesSelectorArgs.SortingCriteria, SortingCriteria);

            if (FilterByCreatedBefore)
            {
                TimeSpan createdBeforeTimeSpan = GetTimeSpanFromFilter(CreatedBeforePeriod, CreatedBefore);
                InstanceArguments = InstanceArguments.WithArgument(FilesSelectorArgs.CreatedBefore,
                    Convert.ToUInt64(createdBeforeTimeSpan.TotalMilliseconds));
            }
            else
            {
                InstanceArguments = InstanceArguments.WithArgument(FilesSelectorArgs.CreatedBefore, null);
            }

            if (FilterByCreatedAfter)
            {
                TimeSpan createdAfterTimeSpan = GetTimeSpanFromFilter(CreatedAfterPeriod, CreatedAfter);
                InstanceArguments = InstanceArguments.WithArgument(FilesSelectorArgs.CreatedAfter,
                    Convert.ToUInt64(createdAfterTimeSpan.TotalMilliseconds));
            }
            else
            {
                InstanceArguments = InstanceArguments.WithArgument(FilesSelectorArgs.CreatedAfter,null);
            }

            if (FilterByEditedBefore)
            {
                TimeSpan editedBeforeTimeSpan = GetTimeSpanFromFilter(EditedBeforePeriod, EditedBefore);
                InstanceArguments = InstanceArguments.WithArgument(FilesSelectorArgs.EditedBefore,
                    Convert.ToUInt64(editedBeforeTimeSpan.TotalMilliseconds));
            }
            else
            {
                InstanceArguments = InstanceArguments.WithArgument(FilesSelectorArgs.EditedBefore, null);
            }

            if (FilterByEditedAfter)
            {
                TimeSpan editedAfterTimeSpan= GetTimeSpanFromFilter(EditedAfterPeriod, EditedAfter);
                InstanceArguments = InstanceArguments.WithArgument(FilesSelectorArgs.EditedAfter,
                    Convert.ToUInt64(editedAfterTimeSpan.TotalMilliseconds));
            }
            else
            {
                InstanceArguments = InstanceArguments.WithArgument(FilesSelectorArgs.EditedAfter, null);
            }
        }

        public override void SetArguments(ArgumentCollection arguments)
        {
            if(arguments == null || !arguments.Any())
                return;

            if (arguments.HasArgument(FilesSelectorArgs.Directory))
                Directory = arguments.GetValue<string>(FilesSelectorArgs.Directory);

            if (arguments.HasArgument(FilesSelectorArgs.Pattern))
                Pattern = arguments.GetValue<string>(FilesSelectorArgs.Pattern);

            if (arguments.HasArgument(FilesSelectorArgs.MaximumFiles))
            {
                if (int.TryParse(arguments.GetValue<string>(FilesSelectorArgs.MaximumFiles), out int maximumFiles))
                {
                    MaximumFiles = maximumFiles;
                }
            }

            if (arguments.HasArgument(FilesSelectorArgs.CreatedBefore))
            {
                var createdBefore = arguments.GetValue<ulong?>(FilesSelectorArgs.CreatedBefore);
                if (TryGetTimePeriodFromMilliseconds(createdBefore, out var timePeriod, out var timeValue))
                {
                    FilterByCreatedBefore = true;
                    CreatedBeforePeriod = timePeriod;
                    CreatedBefore = timeValue;
                }
            }

            if (arguments.HasArgument(FilesSelectorArgs.CreatedAfter))
            {
                var createdAfter = arguments.GetValue<ulong?>(FilesSelectorArgs.CreatedAfter);
                if (TryGetTimePeriodFromMilliseconds(createdAfter, out var timePeriod, out var timeValue))
                {
                    FilterByCreatedAfter = true;
                    CreatedAfterPeriod = timePeriod;
                    CreatedAfter = timeValue;
                }
            }

            if (arguments.HasArgument(FilesSelectorArgs.EditedBefore))
            {
                var editedBefore = arguments.GetValue<ulong?>(FilesSelectorArgs.EditedBefore);
                if (TryGetTimePeriodFromMilliseconds(editedBefore, out var timePeriod, out var timeValue))
                {
                    FilterByEditedBefore = true;
                    EditedBeforePeriod = timePeriod;
                    EditedBefore = timeValue;
                }
            }

            if (arguments.HasArgument(FilesSelectorArgs.EditedAfter))
            {
                var editedAfter = arguments.GetValue<ulong?>(FilesSelectorArgs.EditedAfter);
                if (TryGetTimePeriodFromMilliseconds(editedAfter, out var timePeriod, out var timeValue))
                {
                    FilterByEditedAfter = true;
                    EditedAfterPeriod = timePeriod;
                    EditedAfter = timeValue;
                }
            }

            if (arguments.HasArgument(FilesSelectorArgs.SortingCriteria))
            {
                SortingCriteria = arguments.GetValue<FilesSelectionSortingCriteria>(FilesSelectorArgs.SortingCriteria);
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
