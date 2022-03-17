using bikekeeper.Repository.Abstract;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Repository
{
    public class ParkRepository : BaseRepository<Park>, IParkRepository
    {
        public ParkRepository(BaseContext context) : base(context)
        {

        }
    }
}
