using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using Biker_Keeper_Data.Models;
using bikekeeper.Services.Abstract;
using bikekeeper.Repository.Abstract;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    public class PagesController : GeneralController<PageFilterModel, Pages>
    {
        public PagesController(IGeneralService<PageFilterModel, Pages> IGeneralService, IUnitOfWork UnitOfWork) : base(IGeneralService, UnitOfWork)
        {

        }
    }
}
