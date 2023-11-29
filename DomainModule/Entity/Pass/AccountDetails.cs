using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Entity.Pass
{
    public class AccountDetails
    {
        public static string DefaultPasswordString = "EncryptedPassword";
        public virtual int Id { get; set; }
        public virtual string Account { get; set; }
        public virtual string Password { get; set; }
        public virtual string UserId { get; set;}
        public virtual User User { get; set; }
    }
}
