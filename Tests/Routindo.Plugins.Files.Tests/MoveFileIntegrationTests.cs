using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routindo.Contract;
using Routindo.Contract.Arguments;
using Routindo.Plugins.Files.Components.Actions.MoveFiles;

namespace Routindo.Plugins.Files.Tests
{
    [TestClass]
    public class MoveFileIntegrationTests
    {
        [TestMethod]
        public void MoveFileTestSuccess()
        {
            var destinationPath = Path.Combine(Path.GetTempPath());
            var moveFileAction = new MoveFileAction()
            {
                Id = PluginUtilities.GetUniqueId(),
                DestinationDirectory = destinationPath,
                DestinationPrefix = "RENAMED"
            };
            var sourcePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            using (var file = File.Create(sourcePath))
            {
            }

            Assert.IsTrue(File.Exists(sourcePath));
            Assert.IsFalse(File.Exists(destinationPath));
            var args = new ArgumentCollection((MoveFileActionExecutionArgs.SourceFilePaths, new List<string>() { sourcePath }));

            var result = moveFileAction.Execute(args);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
            Assert.IsFalse(File.Exists(sourcePath));
            destinationPath = Path.Combine(destinationPath, $"RENAMED{Path.GetFileName(sourcePath)}");
            Assert.IsTrue(File.Exists(destinationPath));

            // CleanUp
            File.Delete(destinationPath);
        }

        [TestMethod]
        public void MoveFileFailsWhenUsedByAnotherProcess()
        {
            var destinationPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var moveFileAction = new MoveFileAction()
            {
                Id = PluginUtilities.GetUniqueId(),
                DestinationDirectory = destinationPath
            };
            var sourcePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());


            var createdFileStream = File.Create(sourcePath);
            Assert.IsTrue(File.Exists(sourcePath));
            var args = new ArgumentCollection((MoveFileActionExecutionArgs.SourceFilePaths, sourcePath));
            var result = moveFileAction.Execute(args);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);

            Assert.IsTrue(File.Exists(sourcePath));
            createdFileStream.Close();

            // Cleanup 
            File.Delete(sourcePath);
            Assert.IsFalse(File.Exists(sourcePath));
        }

        [TestMethod]
        public void MoveFileFailsWhenArgumentsNull()
        {
            var moveFileAction = new MoveFileAction()
            {
                Id = PluginUtilities.GetUniqueId()
            };
            var result = moveFileAction.Execute(null);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
        }

        [TestMethod]
        public void MoveFileFailsWhenMissingArgumentSourcePath()
        {
            var moveFileAction = new MoveFileAction()
            {
                Id = PluginUtilities.GetUniqueId(),
                DestinationDirectory = null
            };
            var result = moveFileAction.Execute(null);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
        }

        [TestMethod]
        public void MoveFileFailsWhenMissingArgumentDestinationPath()
        {
            var moveFileAction = new MoveFileAction()
            {
                Id = PluginUtilities.GetUniqueId()
            };
            var args = new ArgumentCollection((MoveFileActionExecutionArgs.SourceFilePaths, ""));
            var result = moveFileAction.Execute(args);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
        }


        [TestMethod]
        public void MoveFileFailsWhenDestinationFileAlreadyExist()
        {
            var destinationPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var moveFileAction = new MoveFileAction()
            {
                Id = PluginUtilities.GetUniqueId(),
                DestinationDirectory = destinationPath
            };
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            using (var sourceFile = File.Create(filePath))
            {
            }

            using (var destinationFile = File.Create(destinationPath))
            {
            }

            var args = new ArgumentCollection((MoveFileActionExecutionArgs.SourceFilePaths, filePath)
                );

            var result = moveFileAction.Execute(args);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);

            // Clean up 
            File.Delete(filePath);
            File.Delete(destinationPath);
        }
    }
}