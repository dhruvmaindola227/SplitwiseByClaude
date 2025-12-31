using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitwiseByClaude
{
    public class CommonUtility
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var validateEmail = new System.Net.Mail.MailAddress(email);
                return validateEmail.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
