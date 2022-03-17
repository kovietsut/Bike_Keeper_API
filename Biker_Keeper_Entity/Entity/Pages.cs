using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Entity
{
    public class Pages : IEntityBase
    {
        public Pages() { }
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AliasPath { get; set; }
        public int? PageOrder { get; set; }       
        public string Icon { get; set; }
        public int? ParentPageId { get; set; }        
    }
}
