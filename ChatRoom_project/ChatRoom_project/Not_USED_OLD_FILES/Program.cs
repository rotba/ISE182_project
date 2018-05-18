using ConsoleApp1.PresintationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ConsoleApp1.BuissnessLayer
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ChatRoom chtrm;
        private static MainMenu menu;
       /* static void Main(string[] args)
        {
            Console.WriteLine("Hello! Please press:" + Environment.NewLine + "'a'- Register" + Environment.NewLine + "'b'- Login" + Environment.NewLine + "'c'- Logout" + Environment.NewLine + "'d'- Retrieve last 10 messages" + Environment.NewLine + "'e'- Display last 20 messages" + Environment.NewLine + "'f'- Display all retrieved messages by a specidic user" + Environment.NewLine + "'g'- To send a message" + Environment.NewLine + "'h'- Exit" + Environment.NewLine + "'i'- Show menu");
            try
            {
                chtrm = new ChatRoom();
                menu = new MainMenu(chtrm);
            }
            catch(Exception e)
            {
               log.Debug("Unexcpeted excpetion while initializing program" + e);
            }
            while (1 == 1)
            {
                try
                {   
                    menu.playInput();
                }
                catch(ToUserException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (ArgumentException e)
                {
                    log.Debug("Argument Exception found: " + e);
                }
               
                catch(Exception e)
                {
                    log.Debug("unexpected error found: " + e);
                }
            }
            
        }*/
    }
}
