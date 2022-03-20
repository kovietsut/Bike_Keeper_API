using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Entity
{
    public class Park: IEntityBase
    {
        public Park() { }
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string Code { get; set; }
        public int CompanyId { get; set; }
        public string Status { get; set; }        
        public DateTime? AvailableTime { get; set; }
        public DateTime? OpenCloseTime { get; set; }
        public int? ParkKindVehicleId { get; set; }
        public string Name { get; set; }
    }
}
