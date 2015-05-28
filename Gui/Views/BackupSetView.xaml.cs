namespace SKnoxConsulting.SafeAndSound.Gui.Views
{
    using Catel.Windows.Controls;
    using SKnoxConsulting.SafeAndSound.Gui.Services;
    using SKnoxConsulting.SafeAndSound.Gui.ViewModels;

    /// <summary>
    /// Interaction logic for BackupSetView.xaml.
    /// </summary>
    public partial class BackupSetView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupSetView"/> class.
        /// </summary>
        public BackupSetView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StateService.RequestBackupSetEdit(this.fc.Id);
        }
    }
}
