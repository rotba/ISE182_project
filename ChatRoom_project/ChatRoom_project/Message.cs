using MileStoneClient.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1.BuissnessLayer
{
    [Serializable]
    public class Message : IMessage
    {
        public Guid Id { get; }
        public string UserName { get; }
        public DateTime Date { get; }
        public string MessageContent { get; }
        public string GroupID { get; }

       // copy constructor
        public Message(IMessage other)
        {
            this.Id = other.Id;
            this.UserName = other.UserName;
            this.Date = other.Date;
            this.MessageContent = other.MessageContent;
            this.GroupID = other.GroupID;
        }
        // constuctor used only for comparision puproses.
        public Message(Guid id, string UserName, DateTime Date, string MessageContent, string GroupID)
        {
            this.Id = id;
            this.UserName = UserName;
            this.Date = Date;
            this.MessageContent = MessageContent;
            this.GroupID = GroupID;
        }

        public override string ToString()
        {
            return String.Format("Message ID:{0}\n" +
                "UserName:{1}\n" +
                "DateTime:{2}\n" +
                "MessageContect:{3}\n" +
                "GroupId:{4}\n"
                , Id, UserName, Date.ToString(), MessageContent, GroupID);
        }   

        //2 messages are equal if GUID is the same in both messages.
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Message m = (Message)obj;
            return (Id == m.Id);
        }
        public override int GetHashCode()
        {
            return (Id.GetHashCode() ^ MessageContent.GetHashCode());
        }


    }
    public class MessageGUIDComp : Comparer<Message>
    {
        /*
         * Returns indiaction to the relativiry of GUID 
         * With respect to GUID.CompareTo
         */
        public override int Compare(Message x, Message y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
    public class MessageDateComp : Comparer<Message>
    {
        /*
         * Returns:
         * 1 if y is earlier than x
         * 0 if their date is equal
         * -1 if y is later
         * if date is equal return is based on guid comparision.
         */
        public override int Compare(Message x, Message y)
        {
            if (!(x.Date.CompareTo(y.Date) == 0))
                return x.Date.CompareTo(y.Date);
            else
                return x.Id.CompareTo(y.Id);
        }
    }
    public class MessageUserComp : Comparer<Message>
    {
        /*
         * Returns:
         * 1 if y is older than x
         * 0 if their dat is equal
         * -1 if y is younger
         */
        public override int Compare(Message x, Message y)
        {
            if (x.GroupID.CompareTo(y.GroupID)!=0)
            {
                return x.GroupID.CompareTo(y.GroupID);
            }
            else
            {
                return x.UserName.CompareTo(y.UserName); 
            }
        }
    }
}