﻿using System;
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

        [Argument(FilesSelectorArgs.Pattern)] public string SearchPattern { get; set; }

        [Argument(FilesSelectorArgs.MaximumFiles, true)] public int MaximumFiles { get; set; }

        [Argument(FilesSelectorArgs.CreatedBefore)] public ulong? CreatedBefore { get; set; }

        [Argument(FilesSelectorArgs.CreatedAfter)] public ulong? CreatedAfter { get; set; }

        [Argument(FilesSelectorArgs.EditedBefore)] public ulong? EditedBefore { get; set; }

        [Argument(FilesSelectorArgs.EditedAfter)] public ulong? EditedAfter { get; set; }

        public virtual List<string> Select()
        {
            try
            {
                var selectedFiles = new DirectoryInfo(DirectoryPath)
                    .GetFiles(SearchPattern, SearchOption.TopDirectoryOnly)
                    .ToList();

                selectedFiles = GetFilesFilteredByTime(selectedFiles);

                selectedFiles = GetSortedFiles(selectedFiles);

                return selectedFiles
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
                selectedFiles = selectedFiles.Where(f =>
                        Convert.ToUInt64(DateTime.Now.Subtract(f.CreationTime).TotalMilliseconds) > CreatedBefore)
                    .ToList();
            }

            if (CreatedAfter.HasValue)
            {
                selectedFiles = selectedFiles.Where(f =>
                        Convert.ToUInt64(DateTime.Now.Subtract(f.CreationTime).TotalMilliseconds) < CreatedAfter)
                    .ToList();
            }

            if (EditedBefore.HasValue)
            {
                selectedFiles = selectedFiles.Where(f =>
                        Convert.ToUInt64(DateTime.Now.Subtract(f.LastWriteTime).TotalMilliseconds) > EditedBefore)
                    .ToList();
            }

            if (EditedAfter.HasValue)
            {
                selectedFiles = selectedFiles.Where(f =>
                        Convert.ToUInt64(DateTime.Now.Subtract(f.LastWriteTime).TotalMilliseconds) < EditedAfter)
                    .ToList();
            }

            return selectedFiles;
        }

        private List<FileInfo> GetSortedFiles(List<FileInfo> selectedFiles)
        {
            if (SortingCriteria != FilesSelectionSortingCriteria.CreationTimeAscending)
            {
                selectedFiles = selectedFiles
                    .OrderBy(f => f.CreationTime)
                    .ToList();
            }
            else if (SortingCriteria != FilesSelectionSortingCriteria.EditionTimeAscending)
            {
                selectedFiles = selectedFiles
                    .OrderBy(f => f.LastWriteTime)
                    .ToList();
            }
            else if (SortingCriteria != FilesSelectionSortingCriteria.CreationTimeDescending)
            {
                selectedFiles = selectedFiles
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();
            }
            else if (SortingCriteria != FilesSelectionSortingCriteria.EditionTimeDescending)
            {
                selectedFiles = selectedFiles
                    .OrderByDescending(f => f.LastWriteTime)
                    .ToList();
            }

            return selectedFiles;
        }
    }
}