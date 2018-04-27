using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatRoom_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int i = 0;
        private ObservableCollection<Message> messages;
        private ICollectionView view_names;
        private ICollectionView view_msg;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ChatRoom chtrm;
        

        Dictionary<int, string> names = new Dictionary<int, string>()
        {
            { 0, "Tomer"},
            { 1, "Dima"},
            { 2, "Rotem"}
        };
        public MainWindow()
        {
            chtrm = new ChatRoom();
            InitializeComponent();           
            messages = new ObservableCollection<Message>();
            //lbMessages.ItemsSource = messages;
            view_names = CollectionViewSource.GetDefaultView(messages);
           // chtrm.register(15, "Tomer");
            chtrm.login(15, "Tomer");
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChatRoomWindow chtrmWindow = new ChatRoomWindow(chtrm, this);
            chtrmWindow.Show();
            this.Hide();
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
    }
}
