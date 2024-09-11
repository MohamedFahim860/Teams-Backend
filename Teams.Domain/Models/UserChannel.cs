using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teams.Domain.Models
{
    internal class UserChannel
    {
        public int Id { get; set; }

        public int UserId { get; set; }//Foreign key
        public User? User { get; set; }//Reference Navigation Property

        public int ChannelId { get; set; }//Foreign key
        public Channel? Channel { get; set; }//Reference Navigation property
 
    }
}
