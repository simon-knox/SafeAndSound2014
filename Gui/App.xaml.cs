namespace SKnoxConsulting.SafeAndSound.Gui
{
    using System.Windows;

    using Catel.Windows;
    using Catel.IoC;
    using SKnoxConsulting.SafeAndSound.Gui.Services.Interfaces;
    using SKnoxConsulting.SafeAndSound.Gui.Services;
    using Catel.MVVM.Services;
    using SKnoxConsulting.SafeAndSound.Gui.ViewModels;
    using SKnoxConsulting.SafeAndSound.Gui.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            Catel.Logging.LogManager.RegisterDebugListener();
#endif

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            // TODO: Using a custom IoC container like Unity? Register it here:
            // Catel.IoC.ServiceLocator.Instance.RegisterExternalContainer(MyUnityContainer);

            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterType<IBackupSetService, BackupSetService>();

            var uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
            uiVisualizerService.Register(typeof(BackupSetViewModel), typeof(BackupSetWindow));

            uiVisualizerService.Register(typeof(ExcludedFilesViewModel), typeof(ExcludedDirectoriesWindow));

            base.OnStartup(e);
        }
    }
}