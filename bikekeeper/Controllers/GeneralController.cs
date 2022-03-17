using bikekeeper.Controllers.Abstract;
using bikekeeper.Models;
using bikekeeper.Repository.Abstract;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bikekeeper.Controllers
{
    public class GeneralController<TCreateViewModel, TModel> : ControllerBase, IGeneralController<TCreateViewModel, TModel>
        where TCreateViewModel : class, IEntityBase
        where TModel : class, IEntityBase
    {
        protected readonly IGeneralService<TCreateViewModel, TModel> iGeneralService;
        protected readonly IUnitOfWork unitOfWork;

        public GeneralController(IGeneralService<TCreateViewModel, TModel> iGeneralService, IUnitOfWork unitOfWork)
        {
            this.iGeneralService = iGeneralService;
            this.unitOfWork = unitOfWork;
        }

        protected int GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Int32.Parse(userId);
        }

        [HttpGet("Get")]
        public virtual JsonResult Get(int id)
        {
            return JsonUtil.Create(iGeneralService.Get(id));
        }

        [HttpGet("GetAll")]
        public virtual JsonResult GetAll()
        {
            return JsonUtil.Create(iGeneralService.GetAll());
        }

        [HttpGet("GetAllPaging")]
        public virtual JsonResult GetAllPaging(int? pageNumber, string searchText, int? pageSize)
        {
            return JsonUtil.Create(iGeneralService.GetAllPaging(pageNumber, searchText, pageSize));
        }

        [HttpPost("Create")]
        public virtual async Task<JsonResult> Create([FromBody] TCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error("");
            }

            return JsonUtil.Create(await iGeneralService.Create(viewModel));
        }

        [HttpPost("Update")]
        public virtual async Task<JsonResult> Update([FromBody] TCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error("");
            }
            return JsonUtil.Create(await iGeneralService.Update(viewModel));
        }

        [HttpPost("Delete")]
        public virtual async Task<JsonResult> Delete([FromBody] TCreateViewModel viewModel)
        {
            return JsonUtil.Create(await iGeneralService.Delete(viewModel));
        }
    }
}
