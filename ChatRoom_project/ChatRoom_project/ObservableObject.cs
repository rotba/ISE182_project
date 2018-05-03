using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ChatRoom_project
{
    public class ObservableObject: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();
        public ListCollectionView view_msg;

        public ObservableObject()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;
            view_msg = CollectionViewSource.GetDefaultView(Messages) as ListCollectionView;
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Messages");
        }

        private string nameFilter = "";
        public string NameFilter
        {
            get
            {
                return nameFilter;
            }
            set
            {
                nameFilter = value;
                OnPropertyChanged("NameFilter");
            }
        }
        private string groupFilter = "";
        public string GroupFilter
        {
            get
            {
                return groupFilter;
            }
            set
            {
                groupFilter = value;
                OnPropertyChanged("GroupFilter");
            }
        }
        private bool sortName = false;
        public bool SortName
        {
            get
            {
                return sortName;
            }
            set
            {
                sortName = value;
                OnPropertyChanged("SortName");
            }
        }
        private bool sortGroup = false;
        public bool SortGroup
        {
            get
            {
                return sortGroup;
            }
            set
            {
                sortGroup = value;
                OnPropertyChanged("SortGroup");
            }
        }
        private bool sortMulty = false;
        public bool SortMulty
        {
            get
            {
                return sortMulty;
            }
            set
            {
                sortMulty = value;
                OnPropertyChanged("SortMulty");
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
