using Biker_Keeper_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Models
{
    public class CompanyModel: IEntityBase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsEnabled { get; set; }       
        public string Location { get; set; }
        public string Message { get; set; }
    }
}
