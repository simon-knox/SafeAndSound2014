using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {    
        public string ProductName
        {
            get 
            {
                return "Safe and Sound 2014";
            }
        }

        public string VersionNumber
        {
            get { return string.Format("Version {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()); }
        }

        public string CopyrightInfo
        {
            get
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                return versionInfo.LegalCopyright;
            }
        }
    }
}
