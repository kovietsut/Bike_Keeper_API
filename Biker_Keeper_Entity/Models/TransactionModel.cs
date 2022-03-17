using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class TransactionModel: IEntityBase
    {
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public int? UserId { get; set; }
        public DateTime? CheckTime { get; set; }
        public int? ParkingCardId { get; set; }
        public int? VehicleTypeId { get; set; }
        public int? CompanyId { get; set; }
        public int? DeviceId { get; set; }
        public string ImageVehicle { get; set; }
        public string Message { get; set; }
        public string QRCode { get; set; }
        public string LicensePlate { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleManufacturer { get; set; }
        public string TransactionType { get; set; }
    }
}
