using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class SearchListCardModel
    {
        public int? ParkId { get; set; }
        public int? CompanyId { get; set; }
        public int? ParkKindVehicleId { get; set; }
        public string SearchText { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
