using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using bikekeeper.Services.Abstract;
using bikekeeper.Models;
using bikekeeper.Repository.Abstract;
using Infrastructure.Utils;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkKindVehiclesController : GeneralController<ParkKindVehicleModel, ParkKindVehicle>
    {
        public ParkKindVehiclesController(IGeneralService<ParkKindVehicleModel, ParkKindVehicle> IGeneralService, IUnitOfWork IUnitOfWork) : base(IGeneralService, IUnitOfWork)
        {

        }

        [HttpPost("Create")]
        public override async Task<JsonResult> Create([FromBody] ParkKindVehicleModel model)
        {
            var checkCode = unitOfWork.RepositoryCRUD<ParkKindVehicle>().Any(x => x.Code == model.Code);
            if (checkCode)
            {
                return JsonUtil.Error("Code existed");
            }
            var checkLocationKindVehicle = unitOfWork.RepositoryCRUD<ParkKindVehicle>().Any(x => x.LocationKindVehicle == model.LocationKindVehicle);
            if (checkLocationKindVehicle)
            {
                return JsonUtil.Error("Location existed");
            }
            var result = await iGeneralService.Create(model);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result);
            }
            return JsonUtil.Error("Create Fail");
        }
    }
}
