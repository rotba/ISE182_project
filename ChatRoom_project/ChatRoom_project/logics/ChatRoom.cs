using ChatRoom_project.DAL;
using ChatRoom_project.logics;
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
        private Request request;
        //private static readonly string DEFAULT_URL = "http://ise172.ise.bgu.ac.il"; // project server url.
        //private static readonly string DEFAULT_URL = "http://localhost";
        //public string Url { get => url; private set => url = value; }
      
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
            //this.url = DEFAULT_URL;
            this.request = new Request(url);
            //this.users = request.retrieveUsers(200,0, null);
            //this.messages = new SortedSet<Message>(new MessageDateComp());
            
            this.LoggedInUser = null;
            //foreach (IMessage msg in request.retrieveMessages(200)) {
            //    messages.Add(new Message(msg));
            //}


        }
    
        /// <summary>
        /// login
        /// </summary>
        public bool login(int g_id, string nickname)
        {   

            if (nickname == null)
                throw new ArgumentNullException("nickname cannot be null");
            if (!isValidNickname(nickname))
            {
                log.Info("Attempted login to invalid nickname" + nickname);
                throw new ToUserException("nickname cant be empty and must hold at most 10 chars");
            }
                User userToLogin = new User(1, g_id, nickname);
            List<IUser> retrievedUsers = request.retrieveUsers(1, g_id, nickname);
            if (retrievedUsers.Count == 0)
            {
                log.Info("Attempted login to invalid user" + userToLogin);
                throw new ToUserException("cannot login to " + userToLogin + " invalid user");
            }
            else
            {
                User retrievedUser = new User(retrievedUsers[0]);
                if (userToLogin.G_id == retrievedUser.G_id && userToLogin.Nickname.Equals(retrievedUser.Nickname))
                {
                    loggedInUser = retrievedUser;
                    return true;
                }
                else
                {
                    log.Debug("Unexpected error when login to  " + userToLogin);
                    throw new ToUserException("unexpected error when trying to login to : " + userToLogin + " please try again");
                }
            }
                        

        }

        

        /*
* Returns the next available userId
*/
/*
        private int getNextUserId()
        {
            int lastId = 0;
            foreach (User user in users) {
                if (user.Id>lastId) {
                    lastId = user.Id;
                }
            }
            return lastId;
        }
*/

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
            if (!isValidNickname(nickname))
            {
                log.Info("Attempted register invalid nickname" + nickname);
                throw new ToUserException("nickname cant be empty and must hold at most 10 chars");
            }
            User userToRegister = new User(1, g_id, nickname);
            List<IUser> retrievedUsers = request.retrieveUsers(1, g_id, nickname);
            if (retrievedUsers.Count != 0)
            {
                log.Info("Attempted to register already registered user " + userToRegister);
                throw new ToUserException("Attempted to register already registered user");
            }
            else
            {
                IUser registeredUser;
                try
                {
                    registeredUser = request.insertUser(userToRegister);
                }
                catch (System.Data.SqlClient.SqlException sqlE)
                {
                    log.Debug("enexpected SQL execption" + sqlE +" while registering user "+userToRegister);
                    throw new ToUserException("unexpected error found please try again");
                }
                catch(ToUserException e)
                {
                    log.Info("while registering user " + userToRegister + " user exception thrown " + e);
                }
                catch (Exception e_1)
                {
                    log.Debug("while registering user " + userToRegister + " unexpected exception thrown " + e_1);
                }

                log.Info("successfully registered user " + registeredUser);
            }
                
            
        }
        /* need to check releveance, maybe only display needed
        //retrieves number amount of messages from server.
        public retrieveMessages(int number)
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
            }
            log.Info(imsg.Count + "Messages were retrieved");
        }
        */
        /****************************************************/
        /*
        //retrieves number amount of messages from server.
        public void retrieveMessages(Guid guid, DateTime date, int number, string nickname, int g_id)
        {
            if (loggedInUser == null)
            {
                log.Info("Attempted to retireve " + number + " messages without initially logging in");
                throw new ToUserException("Cannot retrieve " + number + " messages without initially logging in");
            }
            List<IMessage> imsg = request.retrieveMessages(
                guid, date, number, nickname, g_id
                );
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
            }
            log.Info(imsg.Count + "Messages were retrieved");
        }
        */
        /****************************************************/

        public Message send(string message)
        {
            if (message == null)
            {
                log.Debug("invalid attempt to send null message");
                throw new ArgumentNullException("message cannot be null");
            }
            if (LoggedInUser == null)
            {
                log.Info("Attempted send without initially logging in");
                throw new ToUserException("Cannot send a message without initially logging in");
            }
            if (message != "")
            {
                Message msg = new Message(request.send(message, LoggedInUser));
                // messages.Add(msg);
                log.Info("Message" + msg + " was sent successfully");
                return msg;
            }
            else
            {
                log.Info("Attempted to send empty message");
                return null;
            }
            
        }
        /******************************************************/
        /*
        // main chatroom function - calls requset to get specific needed values from SQL server
        //need to think together
        public SortedSet<Message> displayNMessages(int num)
        {
            if (num < 0)
            {
                log.Debug("invalid attempt to displayNMessages with negative input as num");
                throw new ArgumentOutOfRangeException("display message must recieve non negative integer");
            }
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
        */
        //need to check relevance
        /*
        public SortedSet<Message> retrieveUserMessages(int g_ID, string nickname)
        {
            if(nickname == null)
            {
                log.Debug("invalid attempt to retrieveUserMessages with null nickname");
                throw new ArgumentNullException("nickname cannot be null");
            }
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
                log.Info("Attempted to display a user"+g_ID+" "+nickname +" messages. The user hasn't yet sent a message");
                throw new ToUserException("There are no current messages by the requested user.");
            }
            log.Info("Display a user" + g_ID + " " + nickname + " messages.Total amount: " +ans.Count);
            return ans;
        }
        */

        private bool isValidNickname(string nickname)
        {
            return (nickname.Length <= 10 && nickname.Length>0);
        }

        //used only for test purposes.
        public List<User> getUsers()
        {
            List<User> ans = new List<User>();
            foreach (User u in request.retrieveUsers(-1,0, null ))
            {
                ans.Add(new User(u));
            }
            return ans;
        }
        //for tests only
        public List<Message> getMessages()
        {
            List<Message> ans = new List<Message>();
            foreach (Message m in request.retrieveMessages(0))
            {
                ans.Add(new Message(m));
            }
            return ans;
        }
        //for test use only
        public void deleteUserAndHisMessagesForTestCleanup(User user)
        {
            request.deleteUserAndHisMessagesForTestCleanup(user);
        }
    }
}