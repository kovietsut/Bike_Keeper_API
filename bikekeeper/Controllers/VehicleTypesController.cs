using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using bikekeeper.Models;
using bikekeeper.Services.Abstract;
using bikekeeper.Repository.Abstract;
using Biker_Keeper_Data.Models;
using Infrastructure.Utils;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypesController : GeneralController<VehicleTypeModel, VehicleType>
    {
        public VehicleTypesController(IGeneralService<VehicleTypeModel, VehicleType> IGeneralService, IUnitOfWork IUnitOfWork) : base(IGeneralService, IUnitOfWork)
        {

        }

        [HttpPost("GetListVehicleType")]
        public JsonResult GetListVehicleType([FromBody] SearchListModel model)
        {
            var vehicleType = unitOfWork.RepositoryCRUD<VehicleType>().GetAll();
            var data = vehicleType.Where(x => model.SearchText == "" || x.BrandName.Contains(model.SearchText.Trim()))
            .OrderByDescending(x => x.Id)
            .Select(x => new
            {
                Id = x.Id,
                BrandName = x.BrandName,
            });
            var result = data
            .Skip(((int)model.PageNumber - 1) * (int)model.PageSize)
            .Take((int)model.PageSize);
            if (result != null)
            {
                var totalRecords = data.Count();
                return JsonUtil.Success(result, "Get List Vehicle Type Successful", totalRecords);
            }
            return JsonUtil.Error("Empty Data");
        }
    }
}
