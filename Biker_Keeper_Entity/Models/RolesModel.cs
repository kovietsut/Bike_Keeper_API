using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class RolesModel: IEntityBase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string SearchText { get; set; }
    }
}
