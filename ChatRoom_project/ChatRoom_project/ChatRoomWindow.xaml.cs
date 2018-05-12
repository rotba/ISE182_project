using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ChatRoom_project
{
    /// <summary>
    /// Interaction logic for ChatRoomWindow.xaml
    /// </summary>
    public partial class ChatRoomWindow : Window
    {
        int i = 0;
        ObservableObject _main = new ObservableObject();
        //private ObservableCollection<Message> messages;
        private static Message lastMessage;
        //private ListCollectionView view_msg;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ChatRoom chtrm;
        DispatcherTimer dispatcherTimer;
        private MainWindow mainWindow;
        private Predicate<Message> timeFilter = isOlder;
        private ListSortDirection direction = ListSortDirection.Ascending;
        private bool indexChangedByCode = false;

        public ChatRoomWindow(ChatRoom chtrm, MainWindow mainWindow)
        {
            this.chtrm = chtrm;
            this.mainWindow = mainWindow;
            lastMessage = new Message(new Guid(), null, DateTime.MinValue, null, null);
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();
            InitializeComponent();
            DataContext = _main;
            refreshMessages();
           
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            mainWindow.Close();
            base.OnClosing(e);
        }



        private void dispatcherTimer_Tick(object sender, EventArgs e)

        {
            if (chtrm.LoggedInUser == null)
            {
                
            }
           
            else
            {
                refreshMessages();
            }
        }


        private void refreshMessages()
        {
            Message temp;
            chtrm.retrieveMessages(10);
            SortedSet<Message> toDisplay = chtrm.displayNMessages(20);
            temp = toDisplay.Max;
            toDisplay.RemoveWhere(timeFilter);
            foreach (Message msg in toDisplay)
            {
                try
                {
                    int tGroupID = g_IDToIntAndVerify(msg.GroupID);
                    User tUser = new User(tGroupID, msg.UserName);
                    if (!_main.Users.Contains(tUser))
                    {
                        _main.Users.Add(tUser);
                    }
                    if (!_main.GroupIDs.Contains(tGroupID))
                    {
                        _main.GroupIDs.Add(tGroupID);
                    }
                }
                catch (Exception e)
                {
                    log.Error("unexpected error while adding users and GroupIDs to chatroom viewModel error= "
                        +e);
                }
            }
            toDisplay.ToList().ForEach(_main.Messages.Add);
            lastMessage = temp;
            
        }


        

        private void Button_Click_Asc(object sender, RoutedEventArgs e)
        {
            direction = ListSortDirection.Ascending;
            SortDescriptionCollection sds = new SortDescriptionCollection();
            foreach (var sd in _main.view_msg.SortDescriptions)
            {
                sds.Add(new SortDescription(sd.PropertyName, direction));
            }
            _main.view_msg.SortDescriptions.Clear();
            foreach (var sd in sds)
            {
                _main.view_msg.SortDescriptions.Add(sd);
            }
        }

        private void Button_Click_Desc(object sender, RoutedEventArgs e)
        {
            direction = ListSortDirection.Descending;
            SortDescriptionCollection sds = new SortDescriptionCollection();
            foreach (var sd in _main.view_msg.SortDescriptions)
            {
                sds.Add(new SortDescription(sd.PropertyName, direction));
            }
            _main.view_msg.SortDescriptions.Clear();
            foreach (var sd in sds)
            {
                _main.view_msg.SortDescriptions.Add(sd);
            }
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            chtrm.logout();
            
            mainWindow.Show();
            
            this.Close();
        }
        

        private void RadioButton_Checked_Name(object sender, RoutedEventArgs e)
        {
            _main.view_msg.SortDescriptions.Clear();
            _main.view_msg.SortDescriptions.Add(new SortDescription("UserName", direction));
        }
        private void RadioButton_Unhecked_Name(object sender, RoutedEventArgs e)
        {
            _main.view_msg.SortDescriptions.Clear();
            _main.SortName = false;
        }

        private void RadioButton_Checked_Time(object sender, RoutedEventArgs e)
        {
            _main.view_msg.SortDescriptions.Clear();
            _main.view_msg.SortDescriptions.Add(new SortDescription("Date", direction));
        }

        private void RadioButton_Checked_Multy(object sender, RoutedEventArgs e)
        {
            _main.view_msg.SortDescriptions.Clear();
            _main.view_msg.SortDescriptions.Add(new SortDescription("GroupID", direction));
            _main.view_msg.SortDescriptions.Add(new SortDescription("UserName", direction));
            _main.view_msg.SortDescriptions.Add(new SortDescription("Date", direction));
        }

        
        /*
         * Returns true when is older than lastMessage
         */
        private static bool isOlder(Message message)
        {
            MessageDateComp comp = new MessageDateComp();
            return comp.Compare(message, lastMessage) <= 0;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            chtrm.send(_main.MessageContent);
            _main.MessageContent = "";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        
        
        private void cmbNickName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!indexChangedByCode)
            {
                indexChangedByCode = true;
                _main.CmbGroupIDSelectedIndex = 0;
                indexChangedByCode = false;
                var selectedUser = _main.ComboNickNameSelectedItem;
                if (selectedUser is User)
                {
                    _main.view_msg.Filter = delegate (object item)
                    {
                        if (item is Message)
                        {
                            if (((Message)item).UserName.Equals(((User)selectedUser).Nickname) && ((Message)item).GroupID.Equals(((User)selectedUser).G_id.ToString()))
                            {
                                return true;
                            }
                        }
                        return false;
                    };
                   
                }
                else
                    _main.view_msg.Filter = null;
            }
            else
            {

            }

        }
        private void cmbGroupID_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!indexChangedByCode)
            {
                indexChangedByCode = true;
                _main.CmbNickNameSelectedIndex = 0;
                indexChangedByCode = false;
            
            var selectedID = _main.ComboGroupIDSelectedItem;
                if (selectedID is int)
                {
                    _main.view_msg.Filter = delegate (object item)
                    {
                        if (item is Message)
                        {
                            if (((Message)item).GroupID.Equals(((int)selectedID).ToString()))
                            {
                                return true;
                            }
                        }
                        return false;
                    };
                }

                else
                    _main.view_msg.Filter = null;
            }
            else
            {

            }
        }

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
