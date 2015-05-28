using Catel.MVVM;
using SKnoxConsulting.SafeAndSound.Gui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        public DialogViewModel(UserControl content, DialogButtonModel button1, DialogButtonModel button2 = null, DialogButtonModel button3 = null)
        {
            DialogContent = content;
            Button1Command = button1.ButtonCommand;
            Button1Content = button1.Content;
            IsButton1Default = button1.IsDefault;
            IsButton1Cancel = button1.IsCancel;

            if(button2 != null)
            {
                Button2Command = button2.ButtonCommand;
                Button2Content = button2.Content;
                IsButton2Default = button2.IsDefault;
                IsButton2Cancel = button2.IsCancel;
            }
            else
            {
                IsButton2Visible = false;
            }

            if (button3 != null)
            {
                Button3Command = button3.ButtonCommand;
                Button3Content = button3.Content;
                IsButton3Default = button3.IsDefault;
                IsButton3Cancel = button3.IsCancel;
            }
            else
            {
                IsButton3Visible = false;
            }
        }

        public UserControl DialogContent { get; private set; }

        public ICommand Button1Command { get; private set; }
        public ICommand Button2Command { get; private set; }
        public ICommand Button3Command { get; private set; }

        public bool IsButton2Visible { get; private set; }
        public bool IsButton3Visible { get; private set; }

        public bool IsButton1Default { get; private set; }
        public bool IsButton2Default { get; private set; }
        public bool IsButton3Default { get; private set; }

        public bool IsButton1Cancel { get; private set; }
        public bool IsButton2Cancel{ get; private set; }
        public bool IsButton3Cancel { get; private set; }

        public string Button1Content { get; private set; }
        public string Button2Content { get; private set; }
        public string Button3Content { get; private set; }
    }
}
