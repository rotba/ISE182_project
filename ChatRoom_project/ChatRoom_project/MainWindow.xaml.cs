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
        ObservableModelMainWindow _main;
        

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
            messages = new ObservableCollection<Message>();
            view_names = CollectionViewSource.GetDefaultView(messages);
            this.DataContext = _main;

            // Manually alter window height and width
            this.SizeToContent = SizeToContent.Manual;

            // Automatically resize width relative to content
            this.SizeToContent = SizeToContent.Width;

            // Automatically resize height relative to content
            this.SizeToContent = SizeToContent.Height;

            // Automatically resize height and width relative to content
            this.SizeToContent = SizeToContent.WidthAndHeight;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _main.login(_main.G_IDBox, _main.NicknameBox);
                this.Visibility = Visibility.Collapsed;
                ChatRoomWindow chtrmWindow = new ChatRoomWindow(chtrm, this);
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
            {
                _main.register(_main.G_IDBox, _main.NicknameBox);
                MessageBox.Show("Register successful");
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
}
