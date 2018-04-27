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
        private ICollectionView view_names;
        private ICollectionView view_msg;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ChatRoom chtrm;
        DispatcherTimer dispatcherTimer;
        private MainWindow mainWindow;

        Dictionary<int, string> names = new Dictionary<int, string>()
        {
            { 0, "Tomer"},
            { 1, "Dima"},
            { 2, "Rotem"}
        };
        public ChatRoomWindow(ChatRoom chtrm, MainWindow mainWindow)
        {
            this.chtrm = chtrm;
            this.mainWindow = mainWindow;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();
            InitializeComponent();
            messages = new ObservableCollection<Message>();
            lbMessages.ItemsSource = messages;
            view_names = CollectionViewSource.GetDefaultView(messages);
            // chtrm.register(15, "Tomer");
            chtrm.login(15, "Tomer");
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
           
                chtrm.retrieveMessages(10);
                SortedSet<Message> toDisplay = chtrm.displayNMessages(20);
                toDisplay.ToList().ForEach(messages.Add);
            
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
    }
}
