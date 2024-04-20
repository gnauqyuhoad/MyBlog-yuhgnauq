using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }
        public string? Username { get; set; }

        public string? Password { get; set; }
        public Account(AppUser appUser)
        {
            Username = appUser.UserName;
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
        public Account()
        {
        }
    }
}
