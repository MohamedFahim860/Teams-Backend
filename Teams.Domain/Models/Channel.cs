using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teams.Domain.Models
{
    public class Channel
    {
        public int ChannelId { get; set; }
        public string? ChannelName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDirectChat { get; set; } //to distinguish between direct chats and group channels
        public ICollection <UserChannel>? UserChannel { get; set; }//Reference Navigation property to dependent entity (UserChannel)
        public ICollection<Message>? Message { get; set; } //Reference Navigation property to dependent entity (Message)

    }
}
