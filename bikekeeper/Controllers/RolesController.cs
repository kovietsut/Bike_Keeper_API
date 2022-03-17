using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using bikekeeper.Repository.Abstract;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data.Models;
using Infrastructure.Utils;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : GeneralController<RolesModel, Roles>
    {
        public RolesController(IGeneralService<RolesModel, Roles> IGeneralService, IUnitOfWork IUnitOfWork) : base(IGeneralService, IUnitOfWork)
        {
            
        }
    }
}
