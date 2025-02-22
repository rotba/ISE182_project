﻿using ChatRoom_project.logics;
using ChatRoom_project.Public_Interfaces;
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

namespace ChatRoom_project.PresentationLayer
{
    public class ObservableModelChatRoom: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<IMessage> Messages { get; } = new ObservableCollection<IMessage>();
        public ListCollectionView view_msg;
     
        public ObservableModelChatRoom()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;
            view_msg = CollectionViewSource.GetDefaultView(Messages) as ListCollectionView;
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           
            OnPropertyChanged("Messages");
        }
       
     
       /*
        * Sort properties 
        */
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

        /*
         *Send properties 
         */
        private string messageContent;
        public string MessageContent
        {
            get
            {
                return messageContent;
            }
            set
            {
                messageContent = value;
                OnPropertyChanged("MessageContent");
            }
        }

        /*
         *Filter properties 
         */
        private string nicknameFilterParam;
        public string NicknameFilterParam
        {
            get
            {
                return nicknameFilterParam;
            }
            set
            {
                nicknameFilterParam = value;
                OnPropertyChanged("NicknameFilterParam");
            }
        }

        private string g_IDFilterParam = null;
        public string G_IDFilterParam
        {
            get
            {
                return g_IDFilterParam;
            }
            set
            {
                if (value.Equals(" ") | value.Equals(""))
                    g_IDFilterParam = null;
                else
                    g_IDFilterParam = value;
                OnPropertyChanged("G_IDFilterParam");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChatRoomWindow Observes
        {
            get => default(ChatRoomWindow);
            set
            {
            }
        }
    }
}
