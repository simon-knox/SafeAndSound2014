using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.Models
{
    public enum BackupSetScheduleStatus
    {
        UpToDate,
        Overdue,
        NeverRun,
        LastRunHadErrors
    }
}
