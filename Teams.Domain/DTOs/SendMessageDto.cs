using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.Domain.Models;

namespace Teams.Domain.DTOs
{
    public class SendMessageDto
    {
        public Message message { get; set; }
        public int ReceiverId { get; set; }
    }
}
