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
        private ICollectionView view_names;
        private ICollectionView view_msg;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ChatRoom chtrm;
        DispatcherTimer dispatcherTimer;
        private MainWindow mainWindow;
        private Predicate<Message> timeFilter = isOlder;

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
            view_names = CollectionViewSource.GetDefaultView(messages);
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            view_names.Filter = delegate (object item)
            {
                if (item is Message)
                {
                    if (((Message)item).UserName.Equals("Dima"))
                    {
                        return true;
                    }
                }
                return false;
            };
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            view_names.Filter = delegate (object item)
            {
                if (item is Message)
                {
                    if (((Message)item).UserName.Equals("Rotem"))
                    {
                        return true;
                    }
                }
                return false;
            };
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            chtrm.logout();
            
            mainWindow.Show();
            
            this.Close();
        }
        private static bool isOlder(Message message)
        {
            MessageDateComp comp = new MessageDateComp();
            return comp.Compare(message, lastMessage)<=0;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
