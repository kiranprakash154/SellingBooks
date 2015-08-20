using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Login_System
{
    public class User
    {
        public string firstName;
        public string lastName;
        public string password;
        public string email;
        public List<CartObject> cart;

        public static implicit operator string (User v)
        {
            throw new NotImplementedException();
        }
    }
}