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
        ObservableModelMainWindow _main = new ObservableModelMainWindow();
        this.DataContext = _main;

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
            verifyNickName(nicknameBox.Text);
            chtrm.login(g_IDToIntAndVerify(g_IDBox.Text), nicknameBox.Text);
            nicknameBox.Text = String.Empty;
            g_IDBox.Text = String.Empty;
            ChatRoomWindow chtrmWindow = new ChatRoomWindow(chtrm, this);
            chtrmWindow.Show();
            this.Hide();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {

          // verifyNickName(nicknameBox.Text);
          //  chtrm.register(g_IDToIntAndVerify(g_IDBox.Text), nicknameBox.Text);
          //  MessageBox.Show("Register completed successfully");
          // nicknameBox.Text = String.Empty;
          //  g_IDBox.Text = String.Empty;
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
