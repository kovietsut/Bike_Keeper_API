using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Entity
{
    public class Users : IEntityBase
    {
        public Users() { }
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Status { get; set; }
        public int? WorkingHours { get; set; }
        public int? CompanyId { get; set; }
        public string SecurityStamp { get; set; }
        public string IsGoogleAccount { get; set; }
    }
}
