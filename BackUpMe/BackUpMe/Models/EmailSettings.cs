using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpMe.Models
{
    public class EmailSettings
    {
        public string? FromAddress { get; set; } 

        public string? Password { get; set; }   

        public string? Host { get; set; }

        public int port { get; set; }

        public bool EnableSsl { get; set; }

        public string? ToAddress { get; set; }
    }
}
