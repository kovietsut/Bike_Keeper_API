using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class PageFilterModel: IEntityBase
    {
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string SearchText { get; set; }
        //public int? PageNumber { get; set; }
        //public int? PageSize { get; set; }
    }
}
