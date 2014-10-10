using System;
using NUnit.Framework;
using SKnoxConsulting.SafeAndSound.BackupEngine;

namespace SKnoxConsulting.SafeAndSound.BackupEngineTests
{
    [TestFixture]
    public class BackupErrorTests
    {
        private static string SOURCE = @"C:\Source";
        private static string DESTINATION = @"C:\Destination";
        private static string EXCEPTION_MESSAGE = "Test Exception Message";

        BackupError _backupError;

        [TestFixtureSetUp]
        public void Init()
        {
            _backupError = new BackupError(SOURCE, DESTINATION, new ArgumentException(EXCEPTION_MESSAGE));
        }

        [Test]
        public void BackupErrorSourceIsCorrect()
        {
            Assert.That(_backupError.Source == SOURCE);
        }

        [Test]
        public void BackupDestinationIsCorrect()
        {
            Assert.That(_backupError.Destination == DESTINATION);
        }

        [Test]
        public void BackupErrorMessageIsCorrect()
        {
            Assert.That(_backupError.ErrorMessage == EXCEPTION_MESSAGE);
        }

        [Test]
        public void BackupErrorExceptionIsCorrect()
        {
            Assert.That(_backupError.Ex.GetType() == typeof(ArgumentException));
        }

    }
}
