using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class ParkingCardModel : IEntityBase
    {
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }        
        public string QRCode { get; set; }
        public int? ParkId { get; set; }
    }
}
