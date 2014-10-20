using FirstFloor.ModernUI.Windows.Controls;
using SKnoxConsulting.SafeAndSound.Gui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SKnoxConsulting.SafeAndSound.Gui.Services
{
    public class MessageBoxService : IMessageBoxService
    {

        public MessageBoxResult ShowMessage(string message, string title, MessageBoxButton button)
        {
            return ModernDialog.ShowMessage(message, title, button);
        }
    }
}
