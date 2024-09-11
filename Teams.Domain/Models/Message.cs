using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teams.Domain.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }

        public int ChannelId { get; set; }//Foreign Key
        public Channel? Channel { get; set; }// Reference Navigation Property"  This specifies that EmployeeId belongs to this employee class, that is a foreign key
        public int UserId { get; set; }//Foreign Key
        public User? User { get; set; }// Reference Navigagtion Property for User


    }
}
