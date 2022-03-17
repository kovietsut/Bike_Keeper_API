using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class DeviceModel : IEntityBase
    {
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }        
        public string Code { get; set; }
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public int? ParkId { get; set; }
        public string Status { get; set; }
    }
}
