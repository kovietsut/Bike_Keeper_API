using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Entity
{
    public class Park_Transaction: IEntityBase
    {
        public Park_Transaction() { }

        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public int? UserId { get; set; }
        public DateTime? CheckTime { get; set; }
        public int? ParkingCardId { get; set; }
        public int? VehicleTypeId { get; set; }
        public int? DeviceId { get; set; }
        public string TransactionType { get; set; }
        public string LicensePlate { get; set; }
    }
}
