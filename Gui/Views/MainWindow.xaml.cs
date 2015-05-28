namespace SKnoxConsulting.SafeAndSound.Gui.Views
{
    using Catel.Windows;
    using FirstFloor.ModernUI.Presentation;
    using MahApps.Metro.Controls.Dialogs;
    using SKnoxConsulting.SafeAndSound.Gui.Controls;
    using SKnoxConsulting.SafeAndSound.Gui.Services;
    using SKnoxConsulting.SafeAndSound.Gui.ViewModels;
    using System.Windows;

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
            viewModel.OnThemeChanged += ViewModelThemeChanged;
            DataContext = viewModel;
            DialogService.DialogRequested+=DialogService_DialogRequested;
            
        }

        void DialogService_DialogRequested(object sender, MetroMessageBoxEventArgs e)
        {
            var messageDialogSettings = new MetroDialogSettings();
            //{
            //    AffirmativeButtonText="OK",                
            //    NegativeButtonText="Cancel"
            //};
            var x = this.ShowMessageAsync(e.Title, e.Message, MessageDialogStyle.AffirmativeAndNegative, e.DialogSettings);
            
          
            
           
        }

        private void ViewModelThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            //switch(e.ThemeName)
            //{
            //    case "Dark":
            //        AppearanceManager.Current.ThemeSource = AppearanceManager.DarkThemeSource;
            //        break;
            //    case "Light":
            //        AppearanceManager.Current.ThemeSource = AppearanceManager.LightThemeSource;
                   
            //        break;
            //}
        }       

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {           
            ((MainWindowViewModel)DataContext).SaveAndCloseViewModel();
            base.OnClosing(e);
        }
    }
}
