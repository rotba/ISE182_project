using MileStoneClient.CommunicationLayer;
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
        private readonly int MAX_MESSAGE_LENGTH = 150;
        private string url;
        /// <summary>
        /// used to make sure not to overload the server with requests
        /// </summary>
        private Queue<DateTime> lastNRequests;
        private readonly int N_ALLOWED = 20;
        private readonly int N_SECS = 10;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Request(string url)
        {
            this.url = url;
            lastNRequests = new Queue<DateTime>();
        }
        //validates message, checks server overloading, and sends message to server.
        public IMessage send(string content, User user)
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
                    IMessage iMsg = Communication.Instance.Send(url, user.G_id.ToString(), user.Nickname, content);
                    log.Info("sent send request for message" + content + "for user" + user);
                    return iMsg;
                }
                else
                {
                    //return IMessage iMsg=null;
                    log.Error("Attempted to send more than 20 requests to server in past 10 secs");
                    throw new ToUserException("Illegal attempt to send more than 20 requests to server in past 10 secs");
                }
            }
            else
            {
                //return IMessage iMsg=null;
                log.Error("Attempted to send invalid message");
                throw new ToUserException("illegal attempt to send invalid message, must contain at most 150 characters");
            }
        }
        //check for server overloading, make sure num=10 as in server policy.
        public List<IMessage> retrieveMessages(int num)
        {
            if (num == 10)
            {
                if (isNotOverloading())
                {
                    lastNRequests.Enqueue(DateTime.Now);
                    if (lastNRequests.Count == N_ALLOWED + 1)
                    {
                        lastNRequests.Dequeue();
                    }
                    List<IMessage> msgList = Communication.Instance.GetTenMessages(url);
                    log.Info("sent a GetTenMessages request to server");
                    return msgList;
                }
                else
                {
                    log.Error("Attempted to send more than 20 requests to server in past 10 secs");
                    throw new ToUserException("Illegal attempt to send more than 20 requests to server in past 10 secs");
                }
            }
            else
            {
                log.Error("Illegal attempt to retrieve different amount than 10 last messages");
                throw new ArgumentException("Illegal attempt to retrieve differet amount than 10 last messages");
            }
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
            return (msg.Length <= 150 & msg!="");
        }
    }
}
