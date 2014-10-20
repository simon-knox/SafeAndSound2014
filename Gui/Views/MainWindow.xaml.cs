namespace SKnoxConsulting.SafeAndSound.Gui.Views
{
    using Catel.Windows;
    using SKnoxConsulting.SafeAndSound.Gui.Controls;
    using SKnoxConsulting.SafeAndSound.Gui.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {           
            ((MainWindowViewModel)DataContext).SaveAndCloseViewModel();
            base.OnClosing(e);
        }
    }
}
