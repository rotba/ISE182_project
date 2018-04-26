using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.BuissnessLayer
{
    //exception used to when needed to show error to client user.
    public class ToUserException: Exception
    {
        public ToUserException(string message)
            :base("ERROR: "+message)
        { 
        }
    }
}
