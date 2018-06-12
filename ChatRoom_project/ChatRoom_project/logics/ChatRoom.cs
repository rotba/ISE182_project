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
        private Request request;
        private readonly string saltValue = "1337";
        
        private string nicknameFilterParam = null; //null if no param
        private int g_IDFilterParam = -1;//-1 if no param
        private DateTime lastRetrivedMessageTime;
        private Message lastRetrivedMessage;

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
            this.request = new Request();
            this.LoggedInUser = null;
            lastRetrivedMessage = new Message(new Guid(), null, DateTime.MinValue, null, null);
        }
        /// <summary>
        /// login
        /// </summary>
        public bool login(int g_id, string nickname, string pw)
        {

            if (nickname == null)
                throw new ArgumentNullException("nickname cannot be null");
            if (!isValidNickname(nickname))
            {
                log.Info("Attempted login to invalid nickname" + nickname);
                throw new ToUserException("nickname cant be empty and must hold at most 10 chars");
            }
            string hashedPw = generateSHA256Hash(pw);
            User userToLogin = new User(-1, g_id, nickname,hashedPw);
            List<IUser> retrievedUsers = request.retrieveUsers(1, g_id, nickname);
            if (retrievedUsers.Count == 0)
            {
                log.Info("Attempted login to invalid user" + userToLogin);
                throw new ToUserException("cannot login to " + userToLogin + " invalid user");
            }
            else
            {
                User retrievedUser = new User(retrievedUsers[0]);
                if (!userToLogin.HashedPassword.Equals(retrievedUser.HashedPassword))
                {   
                    
                    log.Info("atempted to login with bad password user = " + userToLogin);
                    log.Info(" user hashedpw = " + userToLogin.HashedPassword + "server Hashed Pw = " + retrievedUser.HashedPassword);
                    throw new ToUserException("Wrong Password");
                }
                else
                {
                    if (userToLogin.G_id == retrievedUser.G_id && userToLogin.Nickname.Equals(retrievedUser.Nickname.TrimEnd(' ')))
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

        public void register(int g_id, string nickname, string pw)
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
            if (pw == null)
                throw new ArgumentNullException("pw cant be null");

            string hashedPw = generateSHA256Hash(pw);
            User userToRegister = new User(-1, g_id, nickname,hashedPw);
            List<IUser> retrievedUsers = request.retrieveUsers(1, g_id, nickname);
            if (retrievedUsers.Count != 0)
            {
                log.Info("Attempted to register already registered user " + userToRegister);
                throw new ToUserException("Attempted to register already registered user");
            }
            else
            {
                IUser registeredUser = null;
                try
                {
                    registeredUser = request.insertUser(userToRegister);
                }
                catch (System.Data.SqlClient.SqlException sqlE)
                {
                    log.Debug("enexpected SQL execption" + sqlE + " while registering user " + userToRegister);
                    throw new ToUserException("unexpected error found please try again");
                }
                catch (ToUserException e)
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
                Message msg = request.send(message, LoggedInUser);
                log.Info("Message" + msg + " was sent successfully");
                return msg;
            }
            else
            {
                log.Info("Attempted to send empty message");
                return null;
            }
            
        }


        // check if diff filter if so changes lastMessage, and update filters
        public void setFilterParameter(string nicknameFilterParam, int g_IDFilterParam)
        {
            if (!this.nicknameFilterParam.Equals(nicknameFilterParam) && this.g_IDFilterParam!=g_IDFilterParam)
            {
                lastRetrivedMessage = new Message(new Guid(), null, DateTime.MinValue, null, null);
            }
            this.nicknameFilterParam = nicknameFilterParam;
            this.g_IDFilterParam = g_IDFilterParam;
           
        }


        public SortedSet<Message> displayNMessages()
        {
            int num = 200;
       
            if (loggedInUser == null)
            {
                log.Info("Attempted to display " + num + " messages without initially logging in");
                throw new ToUserException("Cannot Display messages without initially logging in");
            }
            SortedSet<Message> ans ;
            ans = request.retrieveMessages(default(Guid), lastRetrivedMessage.Date, num, nicknameFilterParam, g_IDFilterParam);
            lastRetrivedMessage = ans.Max;
            return ans;
        }
       

        private bool isValidNickname(string nickname)
        {
            return (nickname.Length <= 10 && nickname.Length>0);
        }

        //create salt added to hased pw

        private string generateSHA256Hash(string input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + saltValue);
            System.Security.Cryptography.SHA256Managed sha256HashString =
                new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256HashString.ComputeHash(bytes);
            return byteArrayToHexString(hash);
        }

        private string byteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }


        //for test use only
        public void deleteUserAndHisMessagesForTestCleanup(User user)
        {
            request.deleteUserAndHisMessagesForTestCleanup(user);
        }
    }
}