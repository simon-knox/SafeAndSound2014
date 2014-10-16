namespace SKnoxConsulting.SafeAndSound.Gui.Views
{
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for BackupSetWindow.xaml.
    /// </summary>
    public partial class BackupSetWindow 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupSetWindow"/> class.
        /// </summary>
        public BackupSetWindow()
            : this(null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupSetWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public BackupSetWindow(BackupSetCollectionWindowViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel, null, DataWindowDefaultButton.OK, true, InfoBarMessageControlGenerationMode.None)
        {
            InitializeComponent();
        }
    }
}
