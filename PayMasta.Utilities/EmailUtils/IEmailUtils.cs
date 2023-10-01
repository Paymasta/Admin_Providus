using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.EmailUtils
{
    public interface IEmailUtils
    {
        bool SendEmail(EmailModel model);
        string ReadEmailformats(string Filename);
        bool SendAccountStatusEmail(string email, string userName, int status);
        bool SendRegistrationEmail(string email, string password, string userName);
        // bool SendJobApplicationEmail(string email, string password, string userName);
        Task<bool> SendEmailBySendGrid();
    }
}
