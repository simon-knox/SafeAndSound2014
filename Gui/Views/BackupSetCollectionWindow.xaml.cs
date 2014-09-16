namespace SKnoxConsulting.SafeAndSound.Gui.Views
{
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for BackupSetCollectionWindow.xaml.
    /// </summary>
    public partial class BackupSetCollectionWindow : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupSetCollectionWindow"/> class.
        /// </summary>
        public BackupSetCollectionWindow()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupSetCollectionWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public BackupSetCollectionWindow(BackupSetCollectionWindowViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
