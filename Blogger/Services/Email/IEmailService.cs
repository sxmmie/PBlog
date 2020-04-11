using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogger.Services.Email
{
    public interface IEmailService
    {
        Task SendMail(string email, string subject, string message);
    }
}
