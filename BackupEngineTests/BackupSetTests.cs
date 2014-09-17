using System;
using NUnit.Framework;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using System.Threading;

namespace SKnoxConsulting.SafeAndSound.BackupEngineTests
{
    [TestFixture]
    public class BackupSetTests
    {
        [Test]
        [Ignore]
        public void RunBackupTest()
        {
            try
            {
                var bs = new BackupSet("TEST", @"D:\TestData", @"D:\TestData2");
                Assert.IsTrue(bs.Status == "Ready.");
                bs.RunBackup();

                while (bs.Status != "Finished.")
                {
                    Thread.Sleep(100);
                }
                Assert.IsTrue(bs.FileCopyCount + bs.FileOverwriteCount + bs.FileSkipCount == bs.TotalFileCount, "File counts don't add up");
                Assert.IsTrue(bs.TotalFileCountMaximum == bs.TotalFileCount, "Didn't react maximum count");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
