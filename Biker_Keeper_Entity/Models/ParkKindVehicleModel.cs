using System;
using Biker_Keeper_Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Models
{
    public class ParkKindVehicleModel : IEntityBase
    {
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string Code { get; set; }
        public string LocationKindVehicle { get; set; }
    }
}
