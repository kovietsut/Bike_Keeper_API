using System;
using Biker_Keeper_Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Models
{
    public class ParkModel: IEntityBase
    {
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? CompanyId { get; set; }
        public string Status { get; set; }       
        public DateTime? AvailableTime { get; set; }
        public DateTime? OpenCloseTime { get; set; }
        public int? ParkKindVehicleId { get; set; }
        public string Message { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string SearchText { get; set; }
    }
}
