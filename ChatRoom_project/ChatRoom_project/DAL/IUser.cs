using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom_project.DAL
{
    public interface IUser
    {
        int Id { get; }
        int G_id { get; }
        string Nickname { get; }
        string HashedPassword { get; }
    }
}
