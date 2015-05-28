namespace SKnoxConsulting.SafeAndSound.Gui.Views
{
    using Catel.Windows;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using ViewModels;

    /// <summary>
    /// Interaction logic for ExcludedDirectoriesWindow.xaml.
    /// </summary>
    public partial class ExcludedDirectoriesWindow 
    {
        private object _dummyNode = null;
        private ExcludedDirectoriesViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludedDirectoriesWindow"/> class.
        /// </summary>
        public ExcludedDirectoriesWindow()
            : this(null)
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludedDirectoriesWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public ExcludedDirectoriesWindow(ExcludedDirectoriesViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
            //this.Buttons = new Button[] { this.OkButton, this.CancelButton };       
        }  

        private void OnLoaded(object sender, RoutedEventArgs e)
        {          
            tvFolders.AddHandler(TreeViewItem.ExpandedEvent,
                new RoutedEventHandler(OnTreeItemExpanded));
            LoadRootNodes();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            tvFolders.RemoveHandler(TreeViewItem.ExpandedEvent,
                new RoutedEventHandler(OnTreeItemExpanded));
        }

        private void OnTreeItemExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.OriginalSource as TreeViewItem;
            if (item != null && item.Items.Count == 1 && item.Items[0] == _dummyNode)
            {
                item.Items.Clear();
                var directory = item.Header as DirectoryViewModel3;
                directory.PopulateSubDirectories();
                foreach (var  subDir in directory.SubDirectories)
                {
                    TreeViewItem subItem = new TreeViewItem();
                    subItem.Header = subDir;
                    subItem.HeaderTemplate = FindResource(
                        "FolderCheckboxTemplate") as DataTemplate;
                    subItem.Items.Add(_dummyNode);
                    item.Items.Add(subItem);
                }
            }
        }

        private void LoadRootNodes()
        {
            tvFolders.Items.Clear();
            foreach (var directory in _viewModel.Items)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = directory;
                item.HeaderTemplate = FindResource("FolderCheckboxTemplate") as DataTemplate;
                item.Items.Add(_dummyNode);
                tvFolders.Items.Add(item);
            }
        }        
    }
}
