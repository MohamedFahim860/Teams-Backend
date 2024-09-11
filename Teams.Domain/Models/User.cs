using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teams.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }//Primary Key
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<UserChannel>? UserChannel { get; set; } //Reference Navigation Property to dependent entity (UserChannel)
        public ICollection <Message>? Message { get; set; } //Reference Naviagatyion Property to dependent entity (Message)

    }
}
