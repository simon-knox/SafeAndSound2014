using System;
using SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using System.Threading;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Text;
//using SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions;

namespace SKnoxConsulting.SafeAndSound.BackupEngineTests
{
    [TestFixture]
    public class CopyFileActionTests
    {
        private string _sourceDirectory;
        private string _destinationDirectory;

        [TestFixtureSetUp]
        public void CreateFilesAndFolders()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _sourceDirectory = Path.Combine(location, "CopyFileActionTestsSource");
            _destinationDirectory = Path.Combine(location, "CopyFileActionTestsDestination");
            Directory.CreateDirectory(_sourceDirectory);
            Directory.CreateDirectory(_destinationDirectory);
            File.CreateText(Path.Combine(_sourceDirectory, "FileToCopy.txt"));
            File.CreateText(Path.Combine(_sourceDirectory, "ExceptionFile.txt"));
            File.CreateText(Path.Combine(_sourceDirectory, "FileToCopyAlreadyExists.txt"));
            File.CreateText(Path.Combine(_destinationDirectory, "FileToCopyAlreadyExists.txt"));        
        }

        [TestFixtureTearDown]
        public void RemoveFilesAndFolders()
        {
            Directory.Delete(_sourceDirectory, true);
            Directory.Delete(_destinationDirectory, true);
        }

        [Test]
        public void CanCreateCopyFileAction()
        {
            var copyFileAction = new CopyFileAction("MySource", "MyDestination");
            Assert.IsTrue(copyFileAction.Source == "MySource", string.Format("Source is {0} instead of MySource", copyFileAction.Source));
            Assert.IsTrue(copyFileAction.Destination == "MyDestination", string.Format("Destination is {0} instead of MyDestination", copyFileAction.Destination));
        }

        [Test]
        public void ExecuteCopiesFile()
        {
            var copyFileAction = new CopyFileAction(Path.Combine(_sourceDirectory, "FileToCopy.txt"), Path.Combine(_destinationDirectory, "FileToCopy.txt"));
            Assert.That(copyFileAction.Execute(), string.Format("File didn't copy {0}", copyFileAction.ErrorMessage));
        }

        [Test]
        public void ExecuteFailsIfSourceDoesNotExist()
        {
            var copyFileAction = new CopyFileAction(Path.Combine(_sourceDirectory, "FileToCopyMissing.txt"), Path.Combine(_destinationDirectory, "FileToCopyMissing.txt"));
            Assert.That(copyFileAction.Execute() == false && copyFileAction.ErrorMessage.Contains("does not exist"));
        }

        [Test]
        public void ExecuteFailsIfDestinationAlreadyExists()
        {
            var copyFileAction = new CopyFileAction(Path.Combine(_sourceDirectory, "FileToCopyAlreadyExists.txt"), Path.Combine(_destinationDirectory, "FileToCopyAlreadyExists.txt"));
            Assert.That(copyFileAction.Execute() == false && copyFileAction.ErrorMessage.Contains("already exists"));
        }

        [Test]
        [Ignore]
        public void ExecuteFailsOnException()
        {
            var copyFileAction = new CopyFileAction(Path.Combine(_sourceDirectory, "ExceptionFile.txt"), Path.Combine(_destinationDirectory, "ExceptionFile.txt"));
            using(StreamWriter s = File.CreateText(copyFileAction.Destination))
            {

            }
            using(FileStream fileStream = new FileStream(copyFileAction.Destination, 
                   FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
               {
                   UnicodeEncoding uniEncoding = new UnicodeEncoding();
                   fileStream.Write(uniEncoding.GetBytes("ABC"),0,3);
                   fileStream.Lock(0, 1);
                   Assert.That(copyFileAction.Execute() == false);

               }
            
        }


        [Test]
        public void CheckActionNameIsCorrect()
        {
            var copyFileAction = new CopyFileAction(Path.Combine(_sourceDirectory, "FileToCopyAlreadyExists.txt"), Path.Combine(_destinationDirectory, "FileToCopyAlreadyExists.txt"));
            Assert.That(copyFileAction.ActionName == "Copy File", "Action Name was not Copy File");
        }



    }
}
