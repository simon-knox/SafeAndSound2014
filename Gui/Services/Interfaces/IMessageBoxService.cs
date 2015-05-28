using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SKnoxConsulting.SafeAndSound.Gui.Services.Interfaces
{
    public interface IMessageBoxService
    {
        MessageBoxResult ShowMessage(string message, string title, MessageBoxButton button);

        //void ShowDialog()
    }
}
