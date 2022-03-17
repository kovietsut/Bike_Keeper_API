using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class SearchListUserModel
    {
        //public List<int> RoleIds { get; set; }
        public string RoleIds { get; set; }
        public int? CompanyId { get; set; }
        public string SearchText { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
