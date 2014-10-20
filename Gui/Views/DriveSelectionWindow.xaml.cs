using Catel.Windows;
using SKnoxConsulting.SafeAndSound.Gui.ViewModels;
using System.Windows.Controls;

namespace SKnoxConsulting.SafeAndSound.Gui.Views
{


    /// <summary>
    /// Interaction logic for BackupSetWindow.xaml.
    /// </summary>
    public partial class DriveSelectionWindow 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriveSelectionWindow"/> class.
        /// </summary>
        public DriveSelectionWindow()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveSelectionWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public DriveSelectionWindow(DriveSelectionViewModel viewModel)
        {
            InitializeComponent();
            Buttons = new Button[] { OkButton, CancelButton };
            DataContext = viewModel;
        }
    }
}
