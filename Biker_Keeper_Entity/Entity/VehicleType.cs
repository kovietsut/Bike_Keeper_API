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
        public string LicensePlate { get; set; }
        public int? QRCodeId { get; set; }
        public string? ImageVehicle { get; set; }
        public bool IsCheckIn { get; set; }

    }
}
