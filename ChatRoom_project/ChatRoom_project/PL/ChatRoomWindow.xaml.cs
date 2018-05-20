using ChatRoom_project.PresentationLayer;
using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace ChatRoom_project.PresentationLayer
{
    /// <summary>
    /// Interaction logic for ChatRoomWindow.xaml
    /// </summary>
    public partial class ChatRoomWindow : Window
    {
        private bool logoutClose = false;
        int i = 0;
        ObservableModelChatRoom observer= new ObservableModelChatRoom();
        private static Message lastMessage;
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
            observer.MessageContent = "";
            DataContext = observer;
            refreshMessages();
           
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            if (!logoutClose) 
                 mainWindow.Close();
            logoutClose = false;
            base.OnClosing(e);
            if (chtrm.LoggedInUser!=null) {
                chtrm.logout();
            }
        }
        
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (chtrm.LoggedInUser == null)
            {
                
            }
            else
            {
                try
                {
                    refreshMessages();
                }
                catch (ToUserException e_1)
                {
                    MessageBox.Show(e_1.Message);
                }
                catch (ArgumentException e_2)
                {
                    log.Debug("Argument Exception found: " + e_2);
                }

                catch (Exception e_3)
                {
                    log.Debug("unexpected error found: " + e_3);
                }
            }
        }

        /*
         *Updates the messages if new ones exist
         */
        private void refreshMessages()
        {
            Message temp;
            try { chtrm.retrieveMessages(10); }
            catch(Exception e)
            {
                log.Debug(e);
            }
           
            SortedSet<Message> toDisplay = chtrm.displayNMessages(20);
            temp = toDisplay.Max;
            toDisplay.RemoveWhere(timeFilter);
            foreach (Message msg in toDisplay)
            {
                try
                {
                    int tGroupID = g_IDToIntAndVerify(msg.GroupID);
                    User tUser = new User(tGroupID, msg.UserName);
                    if (!observer.Users.Contains(tUser))
                    {
                        observer.Users.Add(tUser);
                    }
                    if (!observer.GroupIDs.Contains(tGroupID))
                    {
                        observer.GroupIDs.Add(tGroupID);
                    }
                }
                catch (Exception e)
                {
                    log.Error("unexpected error while adding users and GroupIDs to chatroom viewModel error= "
                        +e);
                }
            }
            toDisplay.ToList().ForEach(observer.Messages.Add);
            lastMessage = temp;// updates the last message to be the current newest message
        }

        /*
         * Sets the direction of the sorting to be ascending
         */
        private void Button_Click_Asc(object sender, RoutedEventArgs e)
        {
            try
            {
                direction = ListSortDirection.Ascending;
                SortDescriptionCollection sds = new SortDescriptionCollection();
                foreach (var sd in observer.view_msg.SortDescriptions)
                {
                    sds.Add(new SortDescription(sd.PropertyName, direction));
                }
                observer.view_msg.SortDescriptions.Clear();
                foreach (var sd in sds)
                {
                    observer.view_msg.SortDescriptions.Add(sd);
                }
            }
            catch (ToUserException e_1)
            {
                MessageBox.Show(e_1.Message);
            }
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
        }
        /*
         * Sets the direction of the sorting to be descending
         */
        private void Button_Click_Desc(object sender, RoutedEventArgs e)
        {
            try
            {
                direction = ListSortDirection.Descending;
                SortDescriptionCollection sds = new SortDescriptionCollection();
                foreach (var sd in observer.view_msg.SortDescriptions)
                {
                    sds.Add(new SortDescription(sd.PropertyName, direction));
                }
                observer.view_msg.SortDescriptions.Clear();
                foreach (var sd in sds)
                {
                    observer.view_msg.SortDescriptions.Add(sd);
                }
            }
            catch (ToUserException e_1)
            {
                MessageBox.Show(e_1.Message);
            }
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
        }

        /*
         * logout from the chatroom and closes the window
         */
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chtrm.logout();
                mainWindow.Show();
                logoutClose = true;
                this.Close();
            }
            catch (ToUserException e_1)
            {
                MessageBox.Show(e_1.Message);
            }
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
        }
        
        private void RadioButton_Checked_Name(object sender, RoutedEventArgs e)
        {
            try
            {
                observer.view_msg.SortDescriptions.Clear();
                observer.view_msg.SortDescriptions.Add(new SortDescription("UserName", direction));
            }
            catch (ToUserException e_1)
            {
                MessageBox.Show(e_1.Message);
            }
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
        }
        private void RadioButton_Unhecked_Name(object sender, RoutedEventArgs e)
        {
            try
            {
                observer.view_msg.SortDescriptions.Clear();
                observer.SortName = false;
            }
            catch (ToUserException e_1)
            {
                MessageBox.Show(e_1.Message);
            }
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
        }

        private void RadioButton_Checked_Time(object sender, RoutedEventArgs e)
        {
            try
            {
                observer.view_msg.SortDescriptions.Clear();
                observer.view_msg.SortDescriptions.Add(new SortDescription("Date", direction));
            }
            catch (ToUserException e_1)
            {
                MessageBox.Show(e_1.Message);
            }
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
        }


        private void RadioButton_Checked_Multy(object sender, RoutedEventArgs e)
        {
            try
            {
                observer.view_msg.SortDescriptions.Clear();
                observer.view_msg.SortDescriptions.Add(new SortDescription("GroupID", direction));
                observer.view_msg.SortDescriptions.Add(new SortDescription("UserName", direction));
                observer.view_msg.SortDescriptions.Add(new SortDescription("Date", direction));
            }
            catch (ToUserException e_1)
            {
                MessageBox.Show(e_1.Message);
            }
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
        }

        
        /*
         * Returns true when is older than lastMessage
         */
        private static bool isOlder(Message message)
        {
            bool isEqual=false;
            try
            {
                MessageDateComp comp = new MessageDateComp();
                isEqual = comp.Compare(message, lastMessage) <= 0;
            }
            catch (ToUserException e_1)
            {
                MessageBox.Show(e_1.Message);
            }
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
            return isEqual;
        }
        /*
         * Sends message
         */
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            IInputElement ii = FocusManager.GetFocusedElement(this);
            DependencyObject b = sender as DependencyObject;
            FocusManager.SetFocusedElement(this, (IInputElement)b);
            try  
            {
                chtrm.send(observer.MessageContent);
                refreshMessages();
            }catch (ToUserException er) {
                MessageBox.Show(er.Message);
            }   
            catch (ArgumentException e_2)
            {
                log.Debug("Argument Exception found: " + e_2);
            }

            catch (Exception e_3)
            {
                log.Debug("unexpected error found: " + e_3);
            }
            observer.MessageContent = "";
            FocusManager.SetFocusedElement(this, ii);
        }

        
        private void cmbNickName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!indexChangedByCode)
            {
                indexChangedByCode = true;
                observer.CmbGroupIDSelectedIndex = 0;
                indexChangedByCode = false;
                var selectedUser = observer.ComboNickNameSelectedItem;
                if (selectedUser is User)
                {
                    observer.view_msg.Filter = delegate (object item)
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
                    observer.view_msg.Filter = null;
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
                observer.CmbNickNameSelectedIndex = 0;
                indexChangedByCode = false;
            
            var selectedID = observer.ComboGroupIDSelectedItem;
                if (selectedID is int)
                {
                    observer.view_msg.Filter = delegate (object item)
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
                    observer.view_msg.Filter = null;
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
