using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Routindo.Contract.Attributes;
using Routindo.Contract.Services;

namespace Routindo.Plugins.Files.Components
{
    public abstract class FilesSelector
    {
        protected abstract ILoggingService Logger { get; }  

        public FilesSelector()
        {
            // _loggingService = ServicesContainer.ServicesProvider.GetLoggingService(nameof(FilesSelector));
        }

        [Argument(FilesSelectorArgs.SortingCriteria)] public FilesSelectionSortingCriteria SortingCriteria { get; set; }

        [Argument(FilesSelectorArgs.Directory, true)] public string DirectoryPath { get; set; }

        [Argument(FilesSelectorArgs.Pattern)] public string SearchPattern { get; set; } = string.Empty;

        [Argument(FilesSelectorArgs.MaximumFiles, true)]
        public int MaximumFiles { get; set; } = 1;

        [Argument(FilesSelectorArgs.CreatedBefore)] public ulong? CreatedBefore { get; set; }

        [Argument(FilesSelectorArgs.CreatedAfter)] public ulong? CreatedAfter { get; set; }

        [Argument(FilesSelectorArgs.EditedBefore)] public ulong? EditedBefore { get; set; }

        [Argument(FilesSelectorArgs.EditedAfter)] public ulong? EditedAfter { get; set; }

        public virtual List<string> Select()
        {
            try
            {
                // Logger.Trace($"Selecting files with Pattern ({SearchPattern}) from directory ({DirectoryPath})");
                var selectedFiles = new DirectoryInfo(DirectoryPath)
                    .GetFiles(SearchPattern, SearchOption.TopDirectoryOnly).ToList();
                    
                selectedFiles = GetFilesFilteredByTime(selectedFiles);
                return GetSortedFiles(selectedFiles)
                    .Take(MaximumFiles)
                    .Select(e => e.FullName).ToList();
             
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return new List<string>();
            }
        }

        private List<FileInfo> GetFilesFilteredByTime(List<FileInfo> selectedFiles)
        {
            if (CreatedBefore.HasValue)
            {
                selectedFiles = selectedFiles
                    .Where(f => f.CreationTime < DateTime.Now.AddMilliseconds(-Convert.ToDouble(CreatedBefore.Value))).ToList();
            }

            if (CreatedAfter.HasValue)
            {
                selectedFiles = selectedFiles
                    .Where(f => f.CreationTime > DateTime.Now.AddMilliseconds(-Convert.ToDouble(CreatedAfter.Value))).ToList();
            }

            if (EditedBefore.HasValue)
            {
                selectedFiles = selectedFiles
                    .Where(f => f.CreationTime < DateTime.Now.AddMilliseconds(-Convert.ToDouble(EditedBefore.Value))).ToList();
            }

            if (EditedAfter.HasValue)
            {
                selectedFiles = selectedFiles
                    .Where(f => f.CreationTime > DateTime.Now.AddMilliseconds(-Convert.ToDouble(EditedAfter.Value))).ToList();
            }

            return selectedFiles;
        }

        private List<FileInfo> GetSortedFiles(List<FileInfo> selectedFiles)
        {
            if (SortingCriteria == FilesSelectionSortingCriteria.CreationTimeAscending)
            {
                return selectedFiles
                    .OrderBy(f => f.CreationTime)
                    .ToList();
            }

            if (SortingCriteria == FilesSelectionSortingCriteria.EditionTimeAscending)
            {
                return selectedFiles
                    .OrderBy(f => f.LastWriteTime)
                    .ToList();
            }

            if (SortingCriteria == FilesSelectionSortingCriteria.CreationTimeDescending)
            {
                return selectedFiles
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();
            }

            if (SortingCriteria == FilesSelectionSortingCriteria.EditionTimeDescending)
            {
                return selectedFiles
                    .OrderByDescending(f => f.LastWriteTime)
                    .ToList();
            }

            return selectedFiles;
        }
    }
}
