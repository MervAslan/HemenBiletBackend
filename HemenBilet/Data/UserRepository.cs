using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HemenBilet.Models;

namespace HemenBilet.Data
{
    public class UserRepository
    {
        private static List<User> _users =  new List<User>();

        static UserRepository()
        {
            _users = new List<User>(){
                new User{UserId = 1,Password="123", UserName="kullanici1",  Email="mail1@gmail.com", Phone="55555555",},
                new User{UserId = 2,Password="124", UserName="kullanici2",  Email="mail2@gmail.com", Phone="55555556",},
                new User{UserId = 3,Password="125", UserName="kullanici3",  Email="mail3@gmail.com", Phone="55555557",},
                new User{UserId = 4,Password="126", UserName="kullanici4",  Email="mail4@gmail.com", Phone="55555558",},
                new User{UserId = 5,Password="127", UserName="kullanici5",  Email="mail5@gmail.com", Phone="55555559",},
                new User{UserId = 6,Password="128", UserName="emine",  Email="mail6@gmail.com", Phone="55555550",},
                new User{UserId = 7,Password="129", UserName="kullanici7",  Email="mail7@gmail.com", Phone="55555551",},
            };
        }

        public static List<User> Users()
        {
            return _users;
        }

        public static void AddUser(User u)
        {
            _users.Add(u);
        }
        public static User GetByIdUser(int id)
        {
            var user =_users.FirstOrDefault(i => i.UserId == id);
            if(user !=null)
                return  user;
            else
                return null;
        }
        public static void DeleteUser(int id)
        {
            var user = _users.FirstOrDefault( i=> i.UserId == id);
            _users.Remove(user);
        }

        public static void EditUser(User u)
        {
            User user = _users.FirstOrDefault(i => i.UserId == u.UserId);
            if(user != null)
            {
                user.UserName = u.UserName;
                user.Email = u.Email;
                user.Phone = u.Phone;
            }

        }
    }
}