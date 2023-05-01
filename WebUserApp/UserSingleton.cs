using DataLayer.DatabaseEntites;

namespace WebUserApp
{
    public class UserSingleton
    {
        public UserSingleton() { }
        public static UserSingleton Instance { get; } = new UserSingleton();
        public User? loggedUser { get; set; }
        

    }
}
