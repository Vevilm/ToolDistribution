using Microsoft.AspNetCore.Identity;

namespace ToolDistribution.Models.DBmodels
{
    public class User : IdentityUser
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }

        public string Status { get; set; }
        
        public User() : base() { }
    }
}
