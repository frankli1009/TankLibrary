using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary.UserIdentity.Models
{
    public class EmailInfo
    {
        public string Description { get; set; }
        public string SmtpClient { get; set; }
        public short SSL { get; set; }
    }
}
