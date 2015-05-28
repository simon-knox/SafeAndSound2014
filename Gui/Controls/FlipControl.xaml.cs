﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Media.Animation;
using SKnoxConsulting.SafeAndSound.Gui.Services;

namespace SKnoxConsulting.SafeAndSound.Gui.Controls
{
    /// <summary>
    /// Interaction logic for FlipControl.xaml
    /// </summary>
    public partial class FlipControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty FrontProperty =
            DependencyProperty.Register("Front", typeof(UIElement), typeof(FlipControl), new UIPropertyMetadata(null));
        public UIElement Front
        {
            get { return (UIElement)GetValue(FrontProperty); }
            set { SetValue(FrontProperty, value); }
        }

        public static readonly DependencyProperty BackProperty =
            DependencyProperty.Register("Back", typeof(UIElement), typeof(FlipControl), new UIPropertyMetadata(null));
        public UIElement Back
        {
            get { return (UIElement)GetValue(BackProperty); }
            set { SetValue(BackProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
           DependencyProperty.Register("Id", typeof(string), typeof(FlipControl), new UIPropertyMetadata(null));
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty FlipDurationProperty =
            DependencyProperty.Register("FlipDuration", typeof(Duration), typeof(FlipControl), new UIPropertyMetadata((Duration)TimeSpan.FromSeconds(0.5)));
        public Duration FlipDuration
        {
            get { return (Duration)GetValue(FlipDurationProperty); }
            set { SetValue(FlipDurationProperty, value); }
        }

        private bool _isFlipped = false;
        public bool IsFlipped
        {
            get { return _isFlipped; }
            private set
            {
                if (value != _isFlipped)
                {
                    _isFlipped = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsFlipped"));
                }
            }
        }

        private IEasingFunction EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };

        public FlipControl()
        {
            StateService.BackupSetViewFlipRequested += StateService_BackupSetViewFlipRequested;
            InitializeComponent();
        }

        void StateService_BackupSetViewFlipRequested(string id)
        {
            if(id == Id)
            {
                Flip();
            }
        }

        public void Flip()
        {
            var animation = new DoubleAnimation()
            {
                Duration = FlipDuration,
                EasingFunction = EasingFunction,
            };
            animation.To = IsFlipped ? 1 : -1;
            transform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            IsFlipped = !IsFlipped;
            OnFlipped(new EventArgs());
        }

        public event EventHandler Flipped;

        protected virtual void OnFlipped(EventArgs e)
        {
            if (this.Flipped != null)
            {
                this.Flipped(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }
    }    
}
