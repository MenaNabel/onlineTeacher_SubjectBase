using Microsoft.AspNetCore.Identity;


namespace OnlineTeacher.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string  Name { get; set; }

    }
}