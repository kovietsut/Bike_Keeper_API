using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Entity
{
    public class ParkKindVehicle: IEntityBase
    {
        public ParkKindVehicle() { }
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string Code { get; set; }
        public string LocationKindVehicle { get; set; }
        public string Status { get; set; }
        public int? ParkId { get; set; }
    }
}
