using ConsoleApp1.BuissnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.PresintationLayer
{
    public class MainMenu
    {
        // write a sentence to explain the user he needs to choose which action to take
        /* a- Register
         * b- Login
         * c- Logout
         * d- Retrieve laste 10 messages
         * e- Display last 20 retrieved messages
         * f- Display all retrieved messages by a specific user
         * g- Wirte and send a message
         * h- Exit
         * i - Show Menu
         */
        private readonly ChatRoom chtrm;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int messagesToDisplay = 20;
        private readonly int messagesToRetrieve = 10;
        public MainMenu(ChatRoom c)
        {
            chtrm = c;
        }

        public void playInput()
        {

            string g_ID;
            string nickname;
            string command = Console.ReadLine();
            switch (command)
            {
                //register
                case "a":
                    Console.WriteLine("Type id");
                    g_ID = Console.ReadLine();
                    int tRegG_ID = g_IDToIntAndVerify(g_ID);
                    Console.WriteLine("Type nickname");
                    nickname = Console.ReadLine();
                    verifyNickName(nickname);
                    chtrm.register(tRegG_ID, nickname,"1234");
                    Console.WriteLine("Register completed!");
                    break;

                //login
                case "b":
                    Console.WriteLine("Type id");
                    g_ID = Console.ReadLine();
                    int tLogG_ID = g_IDToIntAndVerify(g_ID);
                    Console.WriteLine("Type nickname");
                    nickname = Console.ReadLine();
                    verifyNickName(nickname);
                    chtrm.login(tLogG_ID, nickname, "1234");
                    Console.WriteLine("Logged in! Welcome to Tomer and the Design Patterns' chat room!");
                    //Play an intro during a succeful login
                    //System.Media.SoundPlayer player = new System.Media.SoundPlayer(System.IO.Directory.GetCurrentDirectory() + "\\Rick And Morty Intro.wav");
                    //player.Play();

                    break;

                //logout
                case "c":
                    chtrm.logout();
                    Console.WriteLine("Logged out successfully");

                    break;

                //Retrieve laste 10 messages
                case "d":

                    chtrm.retrieveMessages(messagesToRetrieve);
                    Console.WriteLine("Successfully retrieved last 10 messages");
                    break;

                //Display last 20 retrieved messages
                case "e":
                    SortedSet<Message> numMessagesToDisplay = chtrm.displayNMessages(messagesToDisplay);
                    displayMessageList(numMessagesToDisplay);

                    break;

                //Display all retrieved messages by a specific user
                case "f":
                    Console.WriteLine("Type id");
                    g_ID = Console.ReadLine();
                    int tDispG_ID = g_IDToIntAndVerify(g_ID);
                    Console.WriteLine("Type nickname");
                    nickname = Console.ReadLine();
                    verifyNickName(nickname);
                    SortedSet<Message> messagesToDisplayByUser = chtrm.retrieveUserMessages(tDispG_ID, nickname);
                    displayMessageList(messagesToDisplayByUser);
                    break;

                //send
                case "g":
                    Console.WriteLine("Enter message");
                    string text = Console.ReadLine();
                    chtrm.send(text);
                    Console.WriteLine("Message was sent");
                    break;

                //exit
                case "h":
                    Console.WriteLine("bye bye );");
                    System.Threading.Thread.Sleep(3000);
                    chtrm.exit();
                    break;

                //Display menu
                case "i":
                    Console.WriteLine("Hello! Please press:" + Environment.NewLine + "'a'- Register" + Environment.NewLine + "'b'- Login" + Environment.NewLine + "'c'- Logout" + Environment.NewLine + "'d'- Retrieve last 10 messages" + Environment.NewLine + "'e'- Display last 20 messages" + Environment.NewLine + "'f'- Display all retrieved messages by a specidic user" + Environment.NewLine + "'g'- To send a message" + Environment.NewLine + "'h'- Exit" + Environment.NewLine + "'i'- Show menu");
                    break;
                
                //in case there was an invaild entery
                default:
                    Console.WriteLine("Please enter a vaild char");
                    break;
            }
            playInput();
        }

        /// <summary>
        /// displays each element(message) in the list
        /// </summary>
        private void displayMessageList(ICollection<Message> listToDisplay)
        {
            foreach (Message m in listToDisplay)
            {
                Console.WriteLine(m);
            }
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
                throw new ToUserException("group id "+g_ID+" is invalid must be inside the range of -2,147,483,648 to 2,147,483,647.");
            }
            catch (FormatException)
            {
                log.Error("Attempted to enter g_ID: " + g_ID + " that is not only number.");
                throw new ToUserException("The group id " + g_ID +" is not a valid group ID, must contain only numbers");
                 
            }
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
    }
}





