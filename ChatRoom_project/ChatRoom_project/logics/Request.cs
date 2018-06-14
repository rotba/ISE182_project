using ChatRoom_project.Public_Interfaces;
using ChatRoom_project.logics;
using ChatRoom_project.Public_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.BuissnessLayer
{
    //all server requests are sent through this class, class will make sure not to overload server
    //and to validate server policy on each request.
    public class Request
    {   
        private readonly int MAX_MESSAGE_LENGTH = 100;
        private string url;
        /// <summary>
        /// used to make sure not to overload the server with requests
        /// </summary>
        private Queue<DateTime> lastNRequests;
        private readonly int N_ALLOWED = 20;
        private readonly int N_SECS = 10;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MessageHandler mHandler = new MessageHandler();
        private UserHandler uHandler = new UserHandler();

        public Request()
        {
            lastNRequests = new Queue<DateTime>();
        }
        //validates message, checks server overloading, and sends message to server.
        public Message send(string content, User user)
        {
            if (user == null)
                throw new ArgumentNullException("User cannot be null");
            if (content == null)
                throw new ArgumentNullException("content cannot be null");
            if (validate(content))
            {
                if (isNotOverloading())
                {
                    //add time to overloading queue.
                    lastNRequests.Enqueue(DateTime.Now);
                    if (lastNRequests.Count == N_ALLOWED + 1)
                    {
                        lastNRequests.Dequeue();
                    }
                    IMessage iMsg = mHandler.insert(
                        mHandler.convertToDictionary(
                            Guid.NewGuid(),
                            DateTime.Now,
                            user.Id,
                            user.Nickname,
                            user.G_id,
                            content
                            )
                        );
                    log.Info("sent send request for message" + content + "for user" + user);
                    return new Message(iMsg);
                }
                else
                {
                    //return IMessage iMsg=null;
                    log.Error("Attempted to send more than 20 requests to server in past 10 secs");
                    throw new ToUserException("Illegal attempt to send too many requests to server in past 10 secs");
                }
            }
            else
            {
                //return IMessage iMsg=null;
                log.Error("Attempted to send invalid message");
                throw new ToUserException("illegal attempt to send invalid message, must contain at most 100 characters");
            }
        }
        /*
         * Retrieves users from the DB according to the given parameters
         */ 
        public List<IUser> retrieveUsers(int n, int g_id, string nickname)
        {
            return uHandler.retrieve(n, uHandler.convertToDictionary(nickname, g_id,null,-1));
        }

        //check for server overloading, make sure num=10 as in server policy
        public SortedSet<Message> retrieveMessages(Guid guid, DateTime date, int num, string nickname, int g_id) {
                if (isNotOverloading())
                {
                    lastNRequests.Enqueue(DateTime.Now);
                    if (lastNRequests.Count == N_ALLOWED + 1)
                    {
                        lastNRequests.Dequeue();
                    }
                    List<IMessage> msgList =
                        mHandler.retrieve(
                            num,
                            mHandler.convertToDictionary(
                                guid,
                                date,
                                0,
                                nickname,
                                g_id,
                                null)
                            );
                    log.Info("sent a GetTenMessages request to server");
                    if (msgList == null | msgList.Count == 0)
                        return null;
                    return convertToSortedSetOfMessage(msgList);
                }
                else
                {
                    log.Error("Attempted to send more than 20 requests to server in past 10 secs");
                    throw new ToUserException("Illegal attempt to send too many requests to server in past 10 secs");
                }    
        }

        private SortedSet<Message> convertToSortedSetOfMessage(List<IMessage> msgList)
        {
            SortedSet<Message> ans = new SortedSet<Message>(new MessageDateComp());
            foreach (IMessage msg in msgList)
            {
                ans.Add(new Message(msg));
            }
            return ans;
        }
        /*
         * Inerts user to the DB
         */
        public User insertUser(User newUser)
        {
            return new User(uHandler.insert(uHandler.convertToDictionary(newUser.Nickname, newUser.G_id, newUser.HashedPassword, -1)));
        }

        //return true if not overloading server
        private bool isNotOverloading()
        {
           
            if (lastNRequests.Count < N_ALLOWED)
                return true;
            //check that more than N_SECS have past since last N_ALLOWED requests
            else
            {
                DateTime nowtime = DateTime.Now;
                DateTime lasttime = lastNRequests.Peek();
                var seconds = (nowtime - lasttime).TotalSeconds;
                if (seconds <= N_SECS)
                    return false;
                return true;
            }
        }

        private bool validate(string msg)
        {
            return (msg.Length <= 100 & msg!="");
        }
        //**FOR TEST PURPOSES**//
        internal void deleteUserAndHisMessagesForTestCleanup(User user)
        {
            IUser toDelete;
            List<IUser> userList = uHandler.retrieve(1, uHandler.convertToDictionary(user.Nickname, user.G_id, null, -1));
             
            if (userList.Count != 0)
            {
                toDelete = userList.ElementAt(0);
                mHandler.delete(mHandler.convertToDictionary(new Guid(), DateTime.MinValue, toDelete.Id, null, -1, null));
                uHandler.delete(uHandler.convertToDictionary(null, -1, "", toDelete.Id));
            }

        }
    }
}
