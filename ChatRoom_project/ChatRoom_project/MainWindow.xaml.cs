using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        List<Message> items = new List<Message>();
        int i = 0;
        private ObservableCollection<Message> messages;
        Dictionary<int, string> names = new Dictionary<int, string>()
        {
            { 0, "Tomer"},
            { 1, "Dima"},
            { 2, "Rotem"}
        };
        public MainWindow()
        {
            InitializeComponent();
            messages = new ObservableCollection<Message>();
            lbMessages.ItemsSource = messages;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            i = i % 3;
            messages.Insert(messages.Count, new Message(new Guid(), names[i], DateTime.Now, "my name is " + names[i++], "15"));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
