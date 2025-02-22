﻿using ChatRoom_project.Public_Interfaces;
using ChatRoom_project.logics;
using ChatRoom_project.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace ConsoleApp1.BuissnessLayer
{
    /*
     * This class represents the main logic element conrolling the ChatRoom app
     */ 
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
        private string nicknameFilterParam = null; //null if no param
        private int g_IDFilterParam = -1;//-1 if no param
        private DateTime lastRetrivedMessageTime;
        private Message lastRetrivedMessage;
        private readonly string saltValue = "1337";

        /*
         * Represents the user using the ChatRoom app
         */ 
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
            User userToLogin = new User(-1, g_id, nickname,pw);
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


        }
        /*
         * Logs out the current user from the ChatRoom app
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
        /*
         * Closes chatroom
         */ 
        public void exit()
        {
            if (LoggedInUser != null)
                logout();
            Environment.Exit(0);

        }
        /*
         * Registers new user to the DB according to the given prameters
         */ 
        public void register(int g_id, string nickname, string pw)
        {
            if (nickname == null)
                throw new ArgumentNullException("nickname cannot be null");
            if (nickname == "")
                throw new ArgumentOutOfRangeException("nickname cannot be empty");
            if (!isValidNickname(nickname))
            {
                log.Info("Attempted register invalid nickname" + nickname);
                throw new ToUserException("nickname cant be empty and must hold at most 8 chars");
            }
            if (pw == null)
                throw new ArgumentNullException("pw cant be null");

            User userToRegister = new User(-1, g_id, nickname,pw);
            IUser registeredUser = null;
            try
            {
                registeredUser = request.insertUser(userToRegister);
            }catch (SqlException e)
            {
                switch (e.Number)
                {
                    case 2601:
                        log.Debug("SQL execption" + 2601 + " while registering user " + userToRegister);
                        throw new ToUserException("Attempted to register already registered user");
                    case 2627:
                        log.Debug("SQL execption" + 2627 + " while registering user " + userToRegister);
                        throw new ToUserException("Attempted to register already registered user");
                    default:
                        log.Debug("Unexpected SQL execption " + e + " while registering user " + userToRegister);
                        throw new ToUserException("Registration failed because of system issues, Please try again");
                }
            }
            
            catch (Exception e_1)
            {
                log.Debug("while registering user " + userToRegister + " unexpected exception thrown " + e_1);
                throw e_1;
            }
            
            log.Info("successfully registered user " + registeredUser);
        }
      
        /*
         * Sends the given message. Inserts to the DB and returns element representing
         * the sent message
         */ 
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
            if((nicknameFilterParam!=null && !nicknameFilterParam.Equals(this.nicknameFilterParam))||
                !g_IDFilterParam.Equals(this.g_IDFilterParam) ||
                nicknameFilterParam==null && this.nicknameFilterParam!=null
                )
            {
                lastRetrivedMessage = new Message(new Guid(), null, DateTime.MinValue, null, null);
                this.nicknameFilterParam = nicknameFilterParam;
                this.g_IDFilterParam = g_IDFilterParam;
            }
            
           
        }


        public List<IMessage> displayNMessages()
        {
            int num = 200;
       
            if (loggedInUser == null)
            {
                log.Info("Attempted to display " + num + " messages without initially logging in");
                throw new ToUserException("Cannot Display messages without initially logging in");
            }
            SortedSet<Message> aux ;
            List<IMessage> ans = new List<IMessage>();
            aux = request.retrieveMessages(default(Guid), lastRetrivedMessage.Date, num, nicknameFilterParam, g_IDFilterParam);
            if (aux != null) {
                lastRetrivedMessage = aux.Max;
                foreach (Message m in aux) {
                    ans.Add(m);
                }
                return ans;
            }
            return null;
        }
       

        private bool isValidNickname(string nickname)
        {
            return (nickname.Length <= 8 && nickname.Length>0);
        }


        //for test use only
        public void deleteUserAndHisMessagesForTestCleanup(User user)
        {
            request.deleteUserAndHisMessagesForTestCleanup(user);
        }


        public string generateSHA256Hash(string input)
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
    }
}