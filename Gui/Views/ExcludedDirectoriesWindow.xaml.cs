namespace SKnoxConsulting.SafeAndSound.Gui.Views
{
    using Catel.Windows;
    using System.Windows.Controls;
    using ViewModels;

    /// <summary>
    /// Interaction logic for ExcludedDirectoriesWindow.xaml.
    /// </summary>
    public partial class ExcludedDirectoriesWindow 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludedDirectoriesWindow"/> class.
        /// </summary>
        public ExcludedDirectoriesWindow()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludedDirectoriesWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public ExcludedDirectoriesWindow(ExcludedFilesViewModel viewModel)
        {
            InitializeComponent();
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            DataContext = viewModel;
        }
    }
}
