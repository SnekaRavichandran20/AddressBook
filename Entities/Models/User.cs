using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBookApi.Entities
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public UserSecret UserSecret { get; set;}
    }
    public record UserSecret
    {
        [Key]
        public Guid Id { get; set; }
        [Required, EmailAddress(ErrorMessage = "Invalid Username")]
        public string UserName { get; set; }
        [Required, MinLength(6, ErrorMessage = "New Password must be at least 6 characters")]
        public string Password { get; set; }
        public bool Active { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}