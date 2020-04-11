using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogger.Configuration
{
    public class SmtpSettings
    {
        public string From { get; set; }
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
