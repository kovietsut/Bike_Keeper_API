using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class UserData
    {
        public string UserId { get; set; }
        public string  UserName { get; set; }
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? Expires { get; set; }
        public DateTime ExpiresDate { get; set; }
        public string Uid { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
