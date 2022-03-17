using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Entity
{
    public class VehicleType: IEntityBase
    {
        public VehicleType() { }
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string BrandName { get; set; }
        public string Color { get; set; }
    }
}
