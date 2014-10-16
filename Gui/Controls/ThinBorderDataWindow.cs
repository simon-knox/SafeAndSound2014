using Catel.MVVM;
using Catel.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SKnoxConsulting.SafeAndSound.Gui.Controls
{
    public class ThinBorderDataWindow : DataWindow
    {
        static ThinBorderDataWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThinBorderDataWindow),
                new FrameworkPropertyMetadata(typeof(ThinBorderDataWindow)));
        }

        public ThinBorderDataWindow()
            : this(null) { }

        public ThinBorderDataWindow(DataWindowMode dataWindowMode)
            : base(dataWindowMode) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveSelectionWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public ThinBorderDataWindow(IViewModel viewModel)
            : base(viewModel, DataWindowMode.OkCancel, null, DataWindowDefaultButton.OK, true, InfoBarMessageControlGenerationMode.None)
        {
        }

        #region Click events
        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void RestoreClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        public override void OnApplyTemplate()
        {
            Button minimizeButton = GetTemplateChild("minimizeButton") as Button;
            if (minimizeButton != null)
                minimizeButton.Click += MinimizeClick;

            Button restoreButton = GetTemplateChild("restoreButton") as Button;
            if (restoreButton != null)
                restoreButton.Click += RestoreClick;

            Button closeButton = GetTemplateChild("closeButton") as Button;
            if (closeButton != null)
                closeButton.Click += CloseClick;

            base.OnApplyTemplate();
        }

        private void moveRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
