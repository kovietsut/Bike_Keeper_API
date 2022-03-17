using bikekeeper.Repository.Abstract;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Controllers.Abstract
{
    public interface IGeneralController<TCreateViewModel, TModel>
    {
        JsonResult Get(int id);
        JsonResult GetAll();
        JsonResult GetAllPaging(int? pageNumber, string searchText, int? pageSize);
        Task<JsonResult> Create([FromBody] TCreateViewModel viewModel);
        Task<JsonResult> Update([FromBody] TCreateViewModel viewModel);
        Task<JsonResult> Delete([FromBody] TCreateViewModel viewModel);
    }
}
