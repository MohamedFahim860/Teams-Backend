using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teams.Domain.DTOs
{
    public class AddMessageDto
    {
        public string MessageText { get; set; }
        public int ReceiverId { get; set; }
    }
}
