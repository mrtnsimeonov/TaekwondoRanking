using Microsoft.AspNetCore.Identity;

namespace TaekwondoRanking.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
