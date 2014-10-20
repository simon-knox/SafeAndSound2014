using Catel.MVVM;
using SKnoxConsulting.SafeAndSound.BackupEngine.BackupActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    public class ActionLogItemViewModel : ViewModelBase
    {
        BackupAction _backupAction;

        public ActionLogItemViewModel(BackupAction backupAction)
        {
            _backupAction = backupAction;
        }

        public string Status
        {
            get { return _backupAction.Status.ToString(); }
        }

        public string ActionName
        {
            get { return _backupAction.ActionName; }
        }

        public string From
        {
            get
            {
                if (_backupAction is DirectAction)
                {
                    return ((DirectAction)_backupAction).Path;
                }
                else if (_backupAction is CopyFileAction)
                {
                    return ((CopyFileAction)_backupAction).Source;
                }
                return string.Empty;
            }
        }

        public string To
        {
            get
            {
                if (_backupAction is CopyFileAction)
                {
                    return ((CopyFileAction)_backupAction).Destination;
                }
                return string.Empty;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _backupAction.ErrorMessage;
            }
        }

    }
}
