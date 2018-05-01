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
        private ObservableCollection<Message> messages;
        private static Message lastMessage;
        private ListCollectionView view_msg;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ChatRoom chtrm;
        DispatcherTimer dispatcherTimer;
        private MainWindow mainWindow;
        private Predicate<Message> timeFilter = isOlder;
        private ListSortDirection direction = ListSortDirection.Ascending;

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
            messages = new ObservableCollection<Message>();
            lbMessages.ItemsSource = messages;
            view_msg = CollectionViewSource.GetDefaultView(messages) as ListCollectionView;
            
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
            toDisplay.ToList().ForEach(messages.Add);
            lastMessage = temp;
            
        }


        

        private void Button_Click_Asc(object sender, RoutedEventArgs e)
        {
            direction = ListSortDirection.Ascending;
            view_msg.Refresh();
        }

        private void Button_Click_Desc(object sender, RoutedEventArgs e)
        {
            direction = ListSortDirection.Descending;
            view_msg.Refresh();
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            chtrm.logout();
            
            mainWindow.Show();
            
            this.Close();
        }
        

        private void RadioButton_Checked_Name(object sender, RoutedEventArgs e)
        {
            view_msg.SortDescriptions.Add(new SortDescription("Nickname", direction));
        }

        private void RadioButton_Checked_Time(object sender, RoutedEventArgs e)
        {
            view_msg.SortDescriptions.Add(new SortDescription("Date", direction));
        }

        private void RadioButton_Checked_Multy(object sender, RoutedEventArgs e)
        {
            view_msg.CustomSort = new MessageMultyComp();
        }

        private void RadioButton_Checked_Filter_Name(object sender, RoutedEventArgs e)
        {
            view_msg.Filter = delegate (object item)
            {
                if (item is Message)
                {
                    if (((Message)item).UserName.Equals(tb_filter_name.Text))
                    {
                        return true;
                    }
                }
                return false;
            };
        }

        private void RadioButton_Checked_Filter_Group(object sender, RoutedEventArgs e)
        {
            view_msg.Filter = delegate (object item)
            {
                if (item is Message)
                {
                    if (((Message)item).UserName.Equals(tb_filter_group.Text))
                    {
                        return true;
                    }
                }
                return false;
            };
        }
        /*
         * Returns true when is older than lastMessage
         */
        private static bool isOlder(Message message)
        {
            MessageDateComp comp = new MessageDateComp();
            return comp.Compare(message, lastMessage) <= 0;
        }
    }
}
