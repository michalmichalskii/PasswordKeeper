using PasswordKeeper.Domain.Common;

namespace PasswordKeeper.Domain.Entity
{
    public class User : BaseEntity
    {
        public string Site { get; set; }
        public string EmailOrLogin { get; set; }
        public string PasswordString { get; set; }

        public User(int id, string site, string emailOrLogin, string password)
        {   
            Id = id;
            Site = site;
            EmailOrLogin = emailOrLogin;
            PasswordString = password;
        }

    }
}