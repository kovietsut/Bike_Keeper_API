using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using Infrastructure.Utils;
using bikekeeper.Models;
using Microsoft.AspNetCore.Authorization;
using bikekeeper.Repository;
using bikekeeper.Repository.Abstract;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data.Models;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : GeneralController<CompanyModel, Company>
    {
        public CompanyController(IGeneralService<CompanyModel, Company> IGeneralService, IUnitOfWork IUnitOfWork) : base(IGeneralService, IUnitOfWork)
        {
            
        }

        [HttpPost("GetListCompany")]
        public JsonResult GetListCompany([FromBody] SearchListModel model)
        {
            var company = unitOfWork.RepositoryCRUD<Company>().GetAll();                     
            var data = company.Where(x => model.SearchText == "" || x.Code.Contains(model.SearchText.Trim()) ||
                x.Name.Contains(model.SearchText.Trim()) || x.Location.Contains(model.SearchText.Trim()))
            .OrderByDescending(x => x.Id)
            .Select(x => new
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Location = x.Location
            });
            var result = data
            .Skip(((int)model.PageNumber - 1) * (int)model.PageSize)
            .Take((int)model.PageSize);
            if (result != null)
            {
                var totalRecords = data.Count();
                return JsonUtil.Success(result, "Get List Company Successful", totalRecords);
            }
            return JsonUtil.Error("Empty Data");
        }
    }
}
