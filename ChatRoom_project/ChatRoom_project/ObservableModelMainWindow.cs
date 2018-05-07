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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ObservableModelMainWindow(ChatRoom chtrm)
        {
            this.chtrm = chtrm;
            //   Messages.CollectionChanged += Messages_CollectionChanged;

        }
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        private ChatRoom chtrm;
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
                OnPropertyChanged("NicknameBox");
            }
        }

        private string g_IDBox = "";
        public string G_IDBox
        {
            get
            {
                return g_IDBox;
            }
            set
            {
                g_IDBox = value;
                OnPropertyChanged("G_IDBox");
            }
        }

        public void register(String g_ID, String nickname) {
             verifyNickName(nickname);
             chtrm.register(g_IDToIntAndVerify(g_ID), nickname);
        //   MessageBox.Show("Register completed successfully");
             this.G_IDBox = "";
             this.nicknameBox="";

        }

        public void login(String g_ID, String nickname)
        {
            verifyNickName(nickname);
            chtrm.login(g_IDToIntAndVerify(g_ID), nickname);
            //   MessageBox.Show("Register completed successfully");
            this.G_IDBox = "";
            this.nicknameBox = "";

        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// checks that a string is not emptry
        /// </summary>
        private bool verifyNickName(String nickName)
        {
            if (nickName == "")
            {
                log.Error("Attempted to enter empty nickname");
                throw new ToUserException("NickName cannot be empty");
            }
            else
                return true;
        }

        ///<summary>
        ///checks if a string contains only the charcters '0'-'9' and converts it from string to int
        ///<summary>
        private int g_IDToIntAndVerify(String g_ID)
        {
            int result;
            try
            {
                result = Convert.ToInt32(g_ID);
                return result;
            }
            catch (OverflowException)
            {
                log.Error("Attempted to login with g_ID: " + g_ID + " that is outside the range of the Int32 type.");
                throw new ToUserException("group id " + g_ID + " is invalid must be inside the range of -2,147,483,648 to 2,147,483,647.");
            }
            catch (FormatException)
            {
                log.Error("Attempted to enter g_ID: " + g_ID + " that is not only number.");
                throw new ToUserException("The group id " + g_ID + " is not a valid group ID, must contain only numbers");

            }
        }

    }
}
