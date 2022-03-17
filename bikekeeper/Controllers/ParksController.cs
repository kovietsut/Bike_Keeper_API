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
using Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using bikekeeper.Models;
using bikekeeper.Services.Abstract;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParksController : GeneralController<ParkModel, Park>
    {
        public ParksController(IGeneralService<ParkModel, Park> IGeneralService, IUnitOfWork IUnitOfWork) : base(IGeneralService, IUnitOfWork)
        {

        }

        [HttpPost("Create")]
        public override async Task<JsonResult> Create([FromBody] ParkModel model)
        {
            var checkCode = unitOfWork.RepositoryCRUD<Park>().Any(x => x.Code == model.Code);
            if (checkCode)
            {
                return JsonUtil.Error("Code existed");
            }
            var checkName = unitOfWork.RepositoryCRUD<Park>().Any(x => x.Name == model.Name);
            if (checkName)
            {
                return JsonUtil.Error("Name existed");
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