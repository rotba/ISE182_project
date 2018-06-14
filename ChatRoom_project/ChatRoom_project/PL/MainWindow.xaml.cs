using ChatRoom_project.Public_Interfaces;
using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace ChatRoom_project.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ChatRoom chtrm;
        private ObservableModelMainWindow _main;
        private string toHashPW = "";
        private bool toHashPWFlag = false;
        
        
        public MainWindow()
        {
            
            try
            {
                chtrm = new ChatRoom();
                _main = new ObservableModelMainWindow(chtrm);
            }
            catch (Exception e)
            {
                log.Debug("unexpected error found: " + e);
            }
            InitializeComponent();
            this.DataContext = _main;

        }
        

        private void main_grid_loaded(object sender, RoutedEventArgs e)
        {
            
            Grid g = sender as Grid;
            ImageBrush ib = null;
            try
            {
                ib = new ImageBrush(_main.BkImageLocation);
            }
            catch (Exception e_1)
            {
                log.Debug("Unexpected error while loading background image: " + e_1);

            }
            if (ib == null || ib.ImageSource == null)
                g.Background = new SolidColorBrush(Colors.White);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)

        {

            PasswordBox pb = sender as PasswordBox;
            if (verifyPW(pb.Password))
            {
                toHashPWFlag = true;
                toHashPW = chtrm.generateSHA256Hash(pb.Password);

            }
            else
            {
                toHashPWFlag = false;
            }


        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {              
                _main.login(_main.G_IDBox, _main.NicknameBox, toHashPW);
                ChatRoomWindow chtrmWindow = new ChatRoomWindow(chtrm, this);
                this.Hide();
                chtrmWindow.Show();
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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {   if (toHashPWFlag)
                {
                    
                    _main.register(_main.G_IDBox, _main.NicknameBox, toHashPW);
                    MessageBox.Show("Register Successful");
                }
                else
                {
                    throw new ToUserException("Invalid Password. Password must contain charcters and digits only \n and must be 4-16 charcters long");
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

        private bool verifyPW(string pw)
        {
            if (pw == null)
            {
                return false;
            }
            if (pw == "" | pw.Length < 4 | pw.Length > 16)
            {
                return false;
            }

            if (!Regex.IsMatch(pw, @"^[a-zA-Z0-9]+$"))
            {
                return false;
            }
            return true;

        }



    }
}
