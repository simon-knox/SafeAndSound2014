using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SKnoxConsulting.SafeAndSound.Gui.Models
{
    public class DialogButtonModel
    {
        public DialogButtonModel(string content, ICommand buttonCommand, bool isDefault= false, bool isCancel=false)
        {
            Content = content;
            ButtonCommand = buttonCommand;
            IsDefault = isDefault;
            IsCancel = isCancel;
        }

        public bool IsDefault { get; private set; }
        public bool IsCancel { get; private set; }
        public string Content { get; private set; }
        public ICommand ButtonCommand { get; private set; }
    }
}
