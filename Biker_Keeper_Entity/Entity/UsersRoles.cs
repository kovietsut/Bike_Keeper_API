using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Entity
{
    public class UsersRoles : IEntityBase
    {
        public UsersRoles() { }
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }
}
