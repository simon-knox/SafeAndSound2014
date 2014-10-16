namespace SKnoxConsulting.SafeAndSound.Gui.ViewModels
{
    using Catel.MVVM;
    using System.Collections.ObjectModel;
using System.IO;
    using System.Linq;

    /// <summary>
    /// UserControl view model.
    /// </summary>
    public class DriveSelectionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriveSelectionViewModel"/> class.
        /// </summary>
        public DriveSelectionViewModel()
        {
            Drives = new ObservableCollection<DriveInfoViewModel>(DriveInfo.GetDrives().Select(di => new DriveInfoViewModel(di)));
   
            //Console.WriteLine("", ;
            //if (d.IsReady == true)
            //{
            //    Console.WriteLine(", );
            //    Console.WriteLine("  File system: {0}", d.DriveFormat);
            //    Console.WriteLine(
            //        "  Available space to current user:{0, 15} bytes", 
            //        d.AvailableFreeSpace);

            //    Console.WriteLine(
            //        "  Total available space:          {0, 15} bytes",
            //        d.TotalFreeSpace);

            //    Console.WriteLine(
            //        "  Total size of drive:            {0, 15} bytes ",
            //        d.TotalSize);
            //}
        
    
        }

        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "View model title"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        public ObservableCollection<DriveInfoViewModel> Drives
        {
            get;
            set;
        }

        public DriveInfoViewModel SelectedDrive
        {
            get;
            set;
        }
    }
}
