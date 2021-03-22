using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routindo.Contract;
using Routindo.Contract.Arguments;
using Routindo.Plugins.Files.Components.Actions.DeleteFiles;

namespace Routindo.Plugins.Files.Tests
{
    [TestClass]
    public class DeleteFileIntegrationTests
    {
        [TestMethod]
        public void DeleteFileTestSuccess()
        {
            var deleteFileAction = new DeleteFilesAction
            {
                Id = Guid.NewGuid().ToString("N")
            };
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            using (var file = File.Create(filePath))
            {
            }

            Assert.IsTrue(File.Exists(filePath));
            var args = new ArgumentCollection((DeleteFilesActionExecutionArgs.FilesPaths, new List<string> { filePath }));
            var result = deleteFileAction.Execute(args);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
            Assert.IsFalse(File.Exists(filePath));
        }

        [TestMethod]
        public void DeleteFileFailsWhenUsedByAnotherProcess()
        {
            var deleteFileAction = new DeleteFilesAction
            {
                Id = Guid.NewGuid().ToString("N")
            };
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var createdFileStream = File.Create(filePath);
            Assert.IsTrue(File.Exists(filePath));
            var args = new ArgumentCollection((DeleteFilesActionExecutionArgs.FilesPaths, new List<string> { filePath }));
            var result = deleteFileAction.Execute(args);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
            Assert.IsTrue(File.Exists(filePath));
            createdFileStream.Close();
            File.Delete(filePath);
            Assert.IsFalse(File.Exists(filePath));
        }

        [TestMethod]
        public void DeleteFileFailsWhenArgumentsNull()
        {
            var deleteFileAction = new DeleteFilesAction
            {
                Id = Guid.NewGuid().ToString("N")
            };
            var result = deleteFileAction.Execute(null);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
        }

        [TestMethod]
        public void DeleteFileFailsWhenMissingArgument()
        {
            var deleteFileAction = new DeleteFilesAction
            {
                Id = Guid.NewGuid().ToString("N")
            };
            var args = new ArgumentCollection();
            var result = deleteFileAction.Execute(args);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
        }

        [TestMethod]
        public void DeleteFileFailsWhenFileDoesNotExist()
        {
            var deleteFileAction = new DeleteFilesAction
            {
                Id = Guid.NewGuid().ToString("N")
            };
            var filePath = @"\\//?-#.";
            var args = new ArgumentCollection((DeleteFilesActionExecutionArgs.FilesPaths, new List<string> { filePath }));
            var result = deleteFileAction.Execute(args);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
        }
    }
}