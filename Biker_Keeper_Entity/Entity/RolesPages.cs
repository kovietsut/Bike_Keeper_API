using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Entity
{
    public class RolesPages
    {
        public RolesPages() { }
        public int Id { get; set; }
        public string Code { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }

    }
}
