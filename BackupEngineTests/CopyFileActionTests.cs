using System;
using SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using System.Threading;
using NUnit.Framework;
//using SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions;

namespace SKnoxConsulting.SafeAndSound.BackupEngineTests
{
    [TestFixture]
    public class CopyFileActionTests
    {
        [Test]
        public void CanCreateCopyFileAction()
        {
            var copyFileAction = new CopyFileAction("MySource", "MyDestination");
            Assert.IsTrue(copyFileAction.Source == "MySource", string.Format("Source is {0} instead of MySource", copyFileAction.Source));
            Assert.IsTrue(copyFileAction.Destination == "MyDestination", string.Format("Destination is {0} instead of MyDestination", copyFileAction.Destination));
        }



    }
}
