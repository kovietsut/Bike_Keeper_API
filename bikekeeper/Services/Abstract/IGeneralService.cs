using Biker_Keeper_Data;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Services.Abstract
{
    public interface IGeneralService<TCreateViewModel, TModel>
    {
        ResponseModel Get(int id);
        ResponseModel GetAll();
        ResponseModel GetAllPaging(int? pageNumber, string searchText, int? pageSize);
        Task<ResponseModel> Create(TCreateViewModel viewModel);
        Task<ResponseModel> Update(TCreateViewModel viewModel);
        Task<ResponseModel> Delete(TCreateViewModel viewModel);
    }
}
