using SKnoxConsulting.SafeAndSound.Gui.Modules.ViewModels;
namespace SKnoxConsulting.SafeAndSound.Gui.Modules.Views
{

    /// <summary>
    /// Interaction logic for BackupSetView.xaml.
    /// </summary>
    public partial class BackupSetView 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupSetView"/> class.
        /// </summary>
        public BackupSetView(BackupSetViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
