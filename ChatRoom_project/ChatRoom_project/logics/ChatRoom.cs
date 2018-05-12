using ConsoleApp1.PersistentLayer;
using MileStoneClient.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace ConsoleApp1.BuissnessLayer
{
    public class ChatRoom
    {

        /// <summary>
        /// current logged in User
        /// </summary>
        private User loggedInUser;
        /// <summary>
        /// server url
        /// </summary>
        private string url;
        private SortedSet<Message> messages;
        private List<User> users;
        private Request request;
       // private static readonly string DEFAULT_URL = "http://ise172.ise.bgu.ac.il"; // project server url.
        private static readonly string DEFAULT_URL = "http://localhost";
        private readonly UserHandler userHandler;
        private readonly MessageHandler messageHandler;
        public string Url { get => url; private set => url = value; }
      
        public User LoggedInUser {
            get
            {
                if (loggedInUser==null)
                {
                    return null;
                }
                return new User(loggedInUser);
            }
            private set => loggedInUser = value;
            
        }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ChatRoom()
        {
            this.userHandler = new UserHandler();
            this.messageHandler = new MessageHandler();
            this.url = DEFAULT_URL;
            this.users = userHandler.retriveAll();
            this.messages = new SortedSet<Message>(new MessageDateComp());
            messages.UnionWith(messageHandler.retriveAll());
            this.request = new Request(url);
            this.LoggedInUser = null;
           
            
        }
    
        /// <summary>
        /// login
        /// </summary>
        public bool login(int g_id, string nickname)
        {
            if (nickname == null)
                throw new ArgumentNullException("nickname cannot be null");
            User userToLogin = new User(g_id, nickname);
            foreach (User u in users)
            {
                if (u.Equals(userToLogin))
                {
                    LoggedInUser = u;
                    log.Info("Succeccfully logged in" + u);
                    return true;
                }
            }
            log.Info("Attempted login to invalid user" + userToLogin);
            throw new ToUserException("cannot login to user: " + userToLogin + " invalid user");

        }

        public void logout()
        {
            if (LoggedInUser == null)
            {
                log.Info("Attempted logout without initially logging in");
                throw new ToUserException("Cannot Logout without initially logging in");
            }
            else
            {
                log.Info("Succefully logged out of user: "+LoggedInUser);
                LoggedInUser = null;
            }
        }

        public void exit()
        {
            if (LoggedInUser != null)
                logout();
            Environment.Exit(0);



        }

        public void register(int g_id, string nickname)
        {
            if (nickname == null)
                throw new ArgumentNullException("nickname cannot be null");
            if (nickname == "")
                throw new ArgumentOutOfRangeException("nickname cannot be empty");

            User newUser = new User(g_id, nickname);
            //check if user already exists, if so throw error
            if (users.Contains(newUser))
            {
                log.Info("Attempted to register already registered user");
                throw new ToUserException("Attempted to register already registered user");
            }
            //add user to the user list, and save data.
            users.Add(newUser);
            userHandler.save(newUser);
            log.Info("Succeccfully registered" +newUser);
        }
        //retrieves number amount of messages from server.
        public void retrieveMessages(int number)
        {
            if (loggedInUser == null)
            {
                log.Info("Attempted to retireve " + number +" messages without initially logging in");
                throw new ToUserException("Cannot retrieve " + number + " messages without initially logging in");
            }
            List<IMessage> imsg = request.retrieveMessages(number);
            if (imsg.Count == 0)
            {
                log.Info("Attempted to retrieve messages while there are no messages to retrieve");
                throw new ToUserException("No messages to retrieve");
            }
            //converts each element in imsg to Message and adds it to messages list
            foreach (var IMessage in imsg)
            {
                Message addMsgToList = new Message(IMessage);
                messages.Add(addMsgToList);
                messageHandler.save(addMsgToList);
            }
            log.Info(imsg.Count + "Messages were retrieved");
        }

        public void send(string message)
        {
            if (LoggedInUser == null)
            {
                log.Info("Attempted send without initially logging in");
                throw new ToUserException("Cannot send a message without initially logging in");
            }
             Message msg = new Message(request.send(message, LoggedInUser));
             messages.Add(msg);
             messageHandler.save(msg);
             log.Info("Message"+msg +" was sent successfully");
            
        }

        public SortedSet<Message> displayNMessages(int num)
        {
            if (loggedInUser == null)
            {
                log.Info("Attempted to display " + num + " messages without initially logging in");
                throw new ToUserException("Cannot Display " + num + " messages without initially logging in");
            }
            SortedSet<Message> ans = new SortedSet<Message>(new MessageDateComp());
            
            //if the list 'messages' is empty, no messages were retrieved yet
            if (messages.Count == 0)
            {
                log.Info("Attempted to display messages while there are no messages to display");
                throw new ToUserException("No messages to display.");
            }

            //in case the amount of the retrieved messages is less/exactly as the requested amonut to display
            if (messages.Count <= num)
            {
                ans.UnionWith(messages);
                log.Info("Display last " + messages.Count + " messages");
            }
            else
            {
                for (int i = (messages.Count) - num; i < messages.Count; i = i + 1)
                {
                    ans.Add(messages.ElementAt(i));
                }
                log.Info("Display last " + ans.Count + " messages");  
            }
            return ans;
        }
        
        public SortedSet<Message> retrieveUserMessages(int g_ID, string nickname)
        {
            if (loggedInUser == null)
            {
                log.Info("Attempted to Display User: group id :" + g_ID +" Nickname: " +nickname + " messages without initially logging in");
                throw new ToUserException("Cannot Display Users messages without initially logging in");
            }
            SortedSet<Message> ans = new SortedSet<Message>(new MessageDateComp());
            MessageUserComp msgCompByUser = new MessageUserComp();
            String sG_ID = g_ID.ToString();
            Message tFakeMessageToCompareWith = new Message(new Guid(), nickname, new DateTime(), "a", sG_ID);
            
            //adds all the messages that were retrieved of the requested user 
            foreach (Message m in messages)
            {
                if (msgCompByUser.Compare(m, tFakeMessageToCompareWith)==0)
                {
                    ans.Add(m);
                }
            }

            //if the lists is empty, the requested user has not yet sent a message
            if (ans.Count == 0)
            {
                log.Info("Attempted to display a user" +(new User(g_ID,nickname)) +" messages. The user hasn't yet sent a message");
                throw new ToUserException("There are no current messages by the requested user.");
            }
            log.Info("Display a user" + (new User(g_ID, nickname)) + " messages.Total amount: " +ans.Count);
            return ans;
        }
        //used only for test purposes.
        public List<User> getUsers()
        {
            List<User> ans = new List<User>();
            foreach (User u in userHandler.retriveAll())
            {
                ans.Add(new User(u));
            }
            return ans;
        }
        //for tests only
        public List<Message> getMessages()
        {
            List<Message> ans = new List<Message>();
            foreach (Message m in messageHandler.retriveAll())
            {
                ans.Add(new Message(m));
            }
            return ans;
        }
    }
}