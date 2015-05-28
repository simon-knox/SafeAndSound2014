using SKnoxConsulting.SafeAndSound.BackupEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.Services.Interfaces
{
    public interface IBackupSetService
    {
        IEnumerable<IBackupSet> LoadBackupSets();
        void SaveBackupSets(IEnumerable<IBackupSet> backupSets);
    }
}
