using System.Windows;

using Catel.Windows;
using Catel.IoC;
using SKnoxConsulting.SafeAndSound.Gui.Services.Interfaces;
using SKnoxConsulting.SafeAndSound.Gui.Services;
using Catel.MVVM.Services;
using SKnoxConsulting.SafeAndSound.Gui.ViewModels;
using SKnoxConsulting.SafeAndSound.Gui.Views;
using log4net;
using System.Windows.Threading;
using FirstFloor.ModernUI.Presentation;
using System;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SKnoxConsulting.SafeAndSound.Gui
{    

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceLocator _serviceLocator;

        private string _versionNumber;

        private static readonly ILog _log = LogManager.GetLogger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType) ;
        //private static readonly ILog log = LogManager.GetLogger(typeof (Program)) ;
        


        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            _versionNumber = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _log.InfoFormat("Starting Safe and Sound 2015 version {0}", _versionNumber);


            base.OnStartup(e);
#if DEBUG
            Catel.Logging.LogManager.RegisterDebugListener();
           
#endif

            StyleHelper.CreateStyleForwardersForDefaultStyles();

            // TODO: Using a custom IoC container like Unity? Register it here:
            //Catel.IoC.ServiceLocator.Instance.RegisterExternalContainer(MyUnityContainer);

            _serviceLocator = ServiceLocator.Default;
            _serviceLocator.RegisterType<IBackupSetService, BackupSetService>();
            _serviceLocator.RegisterType<IMessageBoxService, MessageBoxService>();
            _serviceLocator.RegisterInstance<ILog>(_log);
            var test = _serviceLocator.GetService(typeof(ILog));
            //test.l

            var uiVisualizerService = _serviceLocator.ResolveType<IUIVisualizerService>();
            uiVisualizerService.Register(typeof(BackupSetViewModel), typeof(BackupSetDialog));
            uiVisualizerService.Register(typeof(ExcludedDirectoriesViewModel), typeof(ExcludedDirectoriesWindow));
            uiVisualizerService.Register(typeof(DriveSelectionViewModel), typeof(DriveSelectionWindow));
            uiVisualizerService.Register(typeof(AboutViewModel), typeof(AboutDialog));

            

            

            var typeFactory = this.GetTypeFactory();
            var shellWindowViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<MainWindowViewModel>();
            shellWindowViewModel.OnThemeChanged += shellWindowViewModel_OnThemeChanged;

            new SKnoxConsulting.SafeAndSound.Gui.Views.MainWindow(shellWindowViewModel).ShowDialog();

           // Bootstrapper bootstrapper = new Bootstrapper();
           // bootstrapper.Run();



            
        }

        void shellWindowViewModel_OnThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            var dark = new Uri("/Resources/SafeAndSoundResourceDarkDictionary.xaml", UriKind.Relative);
            var light = new Uri("/Resources/SafeAndSoundResourceDictionary.xaml", UriKind.Relative);
            switch (e.ThemeName)
            {
                case "Dark":
                    AppearanceManager.Current.ThemeSource = dark;
                    break;
                case "Light":
                    AppearanceManager.Current.ThemeSource = light;

                    break;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _log.InfoFormat("Exiting Safe and Sound 2015 version {0}", _versionNumber);
        }

        
        void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            _log.FatalFormat("{0} {1}", e.Exception.Message, e.Exception.InnerException.Message);
            MessageBox.Show(e.Exception.Message);





            // Prevent default unhandled exception processing

        }
        
    }
}