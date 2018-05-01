using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;




namespace ChatRoom_project
{
    public class ObservableModelMainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

    //    public ObservableModelMainWindow()
    //    {
  //
   //             Messages.CollectionChanged += Messages_CollectionChanged;
    //        
    //    }
     //   public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        private ChatRoom chtrm = new ChatRoom();
        private string nicknameBox = "";
        public string NicknameBox
        {
            get
            {
                return nicknameBox;
            }
            set
            {
                nicknameBox = value;
            }
        }

        private string 

    }
}
