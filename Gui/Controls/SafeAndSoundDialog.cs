using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKnoxConsulting.SafeAndSound.Gui.Controls
{
    public class SafeAndSoundDialog : ModernDialog
    {
        public SafeAndSoundDialog()
        {
            OkButton.Content = "OK";
            CancelButton.Content = "Cancel";          

        }
    }
}
