using System;
using System.Collections.Generic;
using System.Text;

namespace Biker_Keeper_Data.Models
{
    public class CheckOut
    {
        public string Message { get; set; }
        public DateTime? CheckTime { get; set; }
        public double Price { get; set; }
    }
}
