using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class CheckQRModel : IEntityBase
    {
        public String QRCode { get; set; }
        public int Id { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
