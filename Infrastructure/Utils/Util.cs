using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Infrastructure.Utils
{
    public static class Util
    {
        public static bool IsNull(object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }
            return true;
        }

        public static bool IsEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
