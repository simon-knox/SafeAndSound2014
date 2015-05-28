using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKnoxConsulting.SafeAndSound.Gui.Services.Interfaces;
using SKnoxConsulting.SafeAndSound.BackupEngine;
using Catel.Collections;
using Catel.Data;

namespace SKnoxConsulting.SafeAndSound.Gui.Services
{
    public class BackupSetService : IBackupSetService
    {
        private readonly string _path;
 
        public BackupSetService()
        {
            string directory = Catel.IO.Path.GetApplicationDataDirectory("SKnoxConsulting", "SafeAndSound");
 
            _path = Path.Combine(directory, "backupSets.xml");
        }
 
        public IEnumerable<IBackupSet> LoadBackupSets()
        {
            if (!File.Exists(_path))
            {
                return new BackupSet[] { };
            }

            var result = BackupSetCollection.Load(_path, SerializationMode.Xml);
 
            return result.BackupSets.OrderBy(bs=>bs.Name);
        }
 
        public void SaveBackupSets(IEnumerable<IBackupSet> backupSets)
        {
            var backupSetCollection = new BackupSetCollection();
            backupSetCollection.BackupSets.ReplaceRange(backupSets);
            backupSetCollection.SaveTime = DateTime.Now;
            backupSetCollection.Save(_path, SerializationMode.Xml);
        }
    }    
}
