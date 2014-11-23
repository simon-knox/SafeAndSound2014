using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SKnoxConsulting.SafeAndSound.Gui.Util
{
    public class ChangeNotifyingObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public ChangeNotifyingObservableCollection()
            :base()
        {
            HookUpEvents();
        }

        public ChangeNotifyingObservableCollection(IEnumerable<T> collection)
            :base(collection)
        {
            foreach(var item in collection)
            {
                (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(ItemPropertyChanged);                
            }
            HookUpEvents();
        }

        public ChangeNotifyingObservableCollection(List<T> list)
            : base(list)
        {
            foreach (var item in list)
            {
                (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(ItemPropertyChanged);
            }
            HookUpEvents();
        }

        private void HookUpEvents()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(ChangeNotifyingObservableCollectionCollectionChanged);
        }

        private void ChangeNotifyingObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(ItemPropertyChanged);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(ItemPropertyChanged);
                }
            }
        }

        void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {


            if (Application.Current.Dispatcher.CheckAccess())
            {
                NotifyCollectionChangedEventArgs a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                OnCollectionChanged(a);
            }
            else
            {


                Application.Current.Dispatcher.Invoke(() => ItemPropertyChanged(sender, e));//new Action(()=>OnCollectionChanged(a)));
            }
   
            
        }
    }
}
