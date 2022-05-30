using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bus.Helpers
{
    public class HashedPassword
    {
        public string Password { get; set; }
        public string Salt { get; set; }

        public HashedPassword()
        {
        }

        public HashedPassword(string Password, string Salt)
        {
            this.Password = Password;
            this.Salt = Salt;
        }
    }
}
