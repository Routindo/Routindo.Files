using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routindo.Contract.Services;
using Routindo.Plugins.Files.Components;
using Routindo.Plugins.Files.Components.Watchers;

namespace Routindo.Plugins.Files.Tests
{
    [TestClass]
    public class FilesSelectorTests
    {
        private string _testDirectory;
        private string _firstFilePath;
        private string _secondFilePath;
        private string _thirdFilePath;

        [TestInitialize]
        public void Initialize()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            Assert.IsFalse(Directory.Exists(_testDirectory));
            Directory.CreateDirectory(_testDirectory);

            _firstFilePath = Path.Combine(_testDirectory, Path.GetFileName(Path.GetTempFileName()));
            _secondFilePath = Path.Combine(_testDirectory, Path.GetFileName(Path.GetTempFileName()));
            _thirdFilePath = Path.Combine(_testDirectory, Path.ChangeExtension(Path.GetFileName(Path.GetTempFileName()), ".bla"));
            File.WriteAllText(_firstFilePath, "Hello world");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            File.WriteAllText(_secondFilePath, "Hello world 2");
            File.WriteAllText(_thirdFilePath, "Hello world 3");
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (Directory.Exists(_testDirectory))
                Directory.Delete(_testDirectory, true);
        }

        [TestMethod]
        [TestCategory("Integration Test")]
        public void FilterByCreationTimeTest()
        {
            FilesSelector filesSelector = new FilesSelectorMock();
            filesSelector.DirectoryPath = _testDirectory;
            filesSelector.SearchPattern = "*.tmp";
            filesSelector.MaximumFiles = 10;

            // Strict Creation time Filter 
            filesSelector.CreatedAfter = Convert.ToUInt64(TimeSpan.FromSeconds(7).TotalMilliseconds);
            filesSelector.CreatedBefore = Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            var selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(1, selection.Count);
            Assert.AreEqual(_firstFilePath, selection.First());

            // No time creation filter
            filesSelector.CreatedAfter = null; // Convert.ToUInt64(TimeSpan.FromSeconds(7).TotalMilliseconds);
            filesSelector.CreatedBefore = null; // Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(2, selection.Count);

            // Filter only by min time creation
            filesSelector.CreatedAfter = Convert.ToUInt64(TimeSpan.FromSeconds(7).TotalMilliseconds);
            filesSelector.CreatedBefore = null; //Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(2, selection.Count);

            // Filter only by max time creation
            filesSelector.CreatedAfter = Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            filesSelector.CreatedBefore = null;// Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(1, selection.Count);
            Assert.AreEqual(_secondFilePath, selection.First());

            // No time creation filter with sorting oldest first 
            filesSelector.CreatedAfter = null; // Convert.ToUInt64(TimeSpan.FromSeconds(7).TotalMilliseconds);
            filesSelector.CreatedBefore = null; // Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            filesSelector.SortingCriteria = FilesSelectionSortingCriteria.CreationTimeAscending;
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(2, selection.Count);
            Assert.AreEqual(_firstFilePath, selection.First());

            // No time creation filter with sorting newest first 
            filesSelector.SortingCriteria = FilesSelectionSortingCriteria.CreationTimeDescending;
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(2, selection.Count);
            Assert.AreEqual(_secondFilePath, selection.First());
        }

        [TestMethod]
        [TestCategory("Integration Test")]
        public void FilterByEditionTimeTest()
        {
            FilesSelector filesSelector = new FilesSelectorMock();
            filesSelector.DirectoryPath = _testDirectory;
            filesSelector.SearchPattern = "*.tmp";
            filesSelector.MaximumFiles = 10;

            // Strict Edition time Filter 
            filesSelector.EditedAfter = Convert.ToUInt64(TimeSpan.FromSeconds(7).TotalMilliseconds);
            filesSelector.EditedBefore = Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            var selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(1, selection.Count);
            Assert.AreEqual(_firstFilePath, selection.First());

            // No time creation filter
            filesSelector.EditedAfter = null; // Convert.ToUInt64(TimeSpan.FromSeconds(7).TotalMilliseconds);
            filesSelector.EditedBefore = null; // Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(2, selection.Count);

            // Filter only by min time creation
            filesSelector.EditedAfter = Convert.ToUInt64(TimeSpan.FromSeconds(7).TotalMilliseconds);
            filesSelector.EditedBefore = null; //Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(2, selection.Count);

            // Filter only by max time creation
            filesSelector.EditedAfter = Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            filesSelector.EditedBefore = null;// Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(1, selection.Count);
            Assert.AreEqual(_secondFilePath, selection.First());

            // No time creation filter with sorting oldest first 
            filesSelector.EditedAfter = null; // Convert.ToUInt64(TimeSpan.FromSeconds(7).TotalMilliseconds);
            filesSelector.EditedBefore = null; // Convert.ToUInt64(TimeSpan.FromSeconds(3).TotalMilliseconds);
            filesSelector.SortingCriteria = FilesSelectionSortingCriteria.EditionTimeAscending;
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(2, selection.Count);
            Assert.AreEqual(_firstFilePath, selection.First());

            // No time creation filter with sorting newest first 
            filesSelector.SortingCriteria = FilesSelectionSortingCriteria.EditionTimeDescending;
            selection = filesSelector.Select();
            Assert.IsNotNull(selection);
            Assert.AreEqual(2, selection.Count);
            Assert.AreEqual(_secondFilePath, selection.First());
        }
    }

    public class FilesSelectorMock : FilesSelector
    {
        protected override ILoggingService Logger { get; }

        public FilesSelectorMock()
        {
            Logger = ServicesContainer.ServicesProvider.GetLoggingService(nameof(FilesSelectorMock));
        }
    }
}
