using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using Biker_Keeper_Data.Models;
using Microsoft.AspNetCore.Authorization;
using bikekeeper.Services.Abstract;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Auth.Hash;
using System.Text;
using Infrastructure.Utils;
using bikekeeper.Repository.Abstract;
using Google.Api;
using System.Text.RegularExpressions;
using Infrastructure;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : GeneralController<UserModel, Users>
    {
        private readonly IAccountService iAccountService;

        public UsersController(IGeneralService<UserModel, Users> IGeneralService, IUnitOfWork IUnitOfWork, IAccountService IAccountService) : base(IGeneralService, IUnitOfWork)
        {
            iAccountService = IAccountService;
        }

        [HttpPost("Create")]
        public override async Task<JsonResult> Create([FromBody] UserModel model)
        {
            var result = new ResponseModel();
            // Create With Account Google
            if (model.IsGoogleAccount == "on")
            {
                model.Password = null;
            }
            var checkCode = unitOfWork.RepositoryCRUD<Users>().Any(x => x.Code == model.Code);
            if (checkCode)
            {
                return JsonUtil.Error("Code existed");
            }
            var checkName = unitOfWork.RepositoryCRUD<Users>().Any(x => x.Name == model.Name);
            if (checkName)
            {
                return JsonUtil.Error("Name existed");
            }
            if (model.Username == null) return JsonUtil.Error("Username cannot be existed");
            else
            {
                var checkUsername = unitOfWork.RepositoryCRUD<Users>().Any(x => x.Username == model.Username);
                if (checkUsername == true)
                {
                    return JsonUtil.Error("Username existed");
                }
            }
            var isNumber = Regex.IsMatch(model.PhoneNumber, "^[0-9]*$");
            if (model.PhoneNumber.Length != 10 || isNumber == false)
            {
                return JsonUtil.Error("Incorect format phone number");
            }
            if (model.PhoneNumber == null) return JsonUtil.Error("Phone number cannot be empty");
            else
            {
                var checkPhoneNumber = unitOfWork.RepositoryCRUD<Users>().Any(x => x.PhoneNumber == model.PhoneNumber);
                if (checkPhoneNumber == true)
                {
                    return JsonUtil.Error("Phone number existed");
                }
            }
            if (!Util.IsEmail(model.Email))
            {
                return JsonUtil.Error("Incorrect format email");
            }
            else
            {
                model.Email = model.Email.ToLower();
                var checkEmail = unitOfWork.RepositoryCRUD<Users>().Any(x => x.Email == model.Email);
                if (checkEmail)
                {
                    return JsonUtil.Error("Email existed");
                }
            }
            model.CreatedWhen = DateTime.Now;
            model.CreatedBy = GetCurrentUserId();
            model.Status = "Active";
            result = await iGeneralService.Create(model);
            if (result.IsSuccess)
            {
                var user = unitOfWork.RepositoryCRUD<Users>().GetSingle(x => x.Username == model.Username);
                if (model.RoleIds != null)
                {
                    var listRole = model.RoleIds.Split(',');
                    foreach (string roleIdString in listRole)
                    {
                        int roleId = Int32.Parse(roleIdString);
                        UsersRoles userRole = new UsersRoles();
                        userRole.UserId = user.Id;
                        userRole.RoleId = roleId;
                        unitOfWork.RepositoryCRUD<UsersRoles>().Insert(userRole);
                        unitOfWork.Commit();
                    }
                }
            }
            return JsonUtil.Success(result);
        }
       
        [HttpPost("UpdateUser")]
        public JsonResult UpdateUser([FromBody] UserModel model)
        {
            return iAccountService.UpdateUser(model);
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<JsonResult> SignIn([FromBody] UserModel model)
        {
            return await iAccountService.SignIn(model);
        }

        [AllowAnonymous]
        [HttpPost("SignInWithGoogle")]
        public async Task<JsonResult> SignInWithGoogle([FromBody] UserModel model)
        {
            return await iAccountService.SignInWithGoogle(model);
        }

        [HttpPost("GetListUser")]
        public JsonResult GetListUSer([FromBody] SearchListUserModel model)
        {
            var roles = unitOfWork.RepositoryCRUD<Roles>().GetAll();
            var companies = unitOfWork.RepositoryCRUD<Company>().GetAll();
            var users = unitOfWork.RepositoryCRUD<Users>().GetAll();
            var userRoles = unitOfWork.RepositoryCRUD<UsersRoles>().GetAll();

            var joinTables = userRoles
                .Join(users,
                    userRole => userRole.UserId,
                    user => user.Id,
                    (userRole, user) =>
                    new
                    {
                        Id = user.Id,
                        UserId = userRole.UserId,
                        RoleId = userRole.RoleId,
                        Code = user.Code,
                        Status = user.Status,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Username = user.Username,
                        DateOfBirth = user.DateOfBirth,
                        WorkingHours = user.WorkingHours,
                        CompanyId = user.CompanyId,
                        CreatedWhen = user.CreatedWhen
                    })
                .Join(companies,
                    user => user.CompanyId,
                    company => company.Id,
                    (user, company) =>
                    new
                    {
                        CompanyName = company.Name,
                        Id = user.Id,
                        RoleId = user.RoleId,
                        Code = user.Code,
                        Status = user.Status,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Username = user.Username,
                        DateOfBirth = user.DateOfBirth,
                        WorkingHours = user.WorkingHours,
                        CompanyId = user.CompanyId,
                        CreatedWhen = user.CreatedWhen
                    })
                .Join(roles,
                    user => user.RoleId,
                    role => role.Id,
                    (user, role) =>
                    new
                    {
                        Id = user.Id,
                        RoleName = role.Name,
                        CompanyName = user.CompanyName,
                        RoleId = user.RoleId,
                        Code = user.Code,
                        Status = user.Status,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        Username = user.Username,
                        DateOfBirth = user.DateOfBirth,
                        WorkingHours = user.WorkingHours,
                        CompanyId = user.CompanyId,
                        CreatedWhen = user.CreatedWhen
                    });
            // Better performance, return object instead of array
            var data = joinTables.Where(x => (model.RoleIds.Contains(x.RoleId.ToString()) || model.RoleIds == "") && (x.CompanyId == model.CompanyId || model.CompanyId == null) &&
                    (model.SearchText == "" || x.Code.Contains(model.SearchText.Trim()) || x.Status.Contains(model.SearchText.Trim()) ||
                    x.PhoneNumber.Contains(model.SearchText.Trim()) || x.Username.Contains(model.SearchText.Trim()) || x.Email.Contains(model.SearchText.Trim()) ||
                    x.DateOfBirth.ToString().Contains(model.SearchText.Trim()) || x.CompanyName.Contains(model.SearchText.Trim()) ||
                    x.WorkingHours.ToString().Contains(model.SearchText.Trim()) || x.RoleName.Contains(model.SearchText.Trim())))
            .GroupBy(x => x.Id)
            .OrderByDescending(x => x.First().CreatedWhen)
            .Select(x => new
            {
                Id = x.Key,
                RoleName = string.Join(", ", x.Select(role => role.RoleName)),
                CompanyName = x.First().CompanyName,
                RoleIds = string.Join(", ", x.Select(role => role.RoleId)),
                Code = x.First().Code,
                Status = x.First().Status,
                PhoneNumber = x.First().PhoneNumber,
                Email = x.First().Email,
                Username = x.First().Username,
                DateOfBirth = x.First().DateOfBirth,
                WorkingHours = x.First().WorkingHours,
                CompanyId = x.First().CompanyId
            });

            var result = data
            .Skip(((int)model.PageNumber - 1) * (int)model.PageSize)
            .Take((int)model.PageSize);
            if (result != null)
            {
                var totalRecords = data.Count();
                return JsonUtil.Success(result, "Get List User Successful", totalRecords);
            }
            return JsonUtil.Error("Empty Data");
        }

        [HttpGet("GetUserByCompany")]
        public JsonResult GetListUSer(int companyId)
        {
            var data = unitOfWork.RepositoryCRUD<Users>().GetAll().Where(x => x.CompanyId == companyId);
            if (data != null)
            {
                var totalRecords = data.Count();
                return JsonUtil.Success(data, "Get User By Company", totalRecords);
            }
            return JsonUtil.Error("Empt Data");
        }

        [HttpPost("Delete")]
        public override async Task<JsonResult> Delete([FromBody] UserModel model)
        {
            var user = unitOfWork.RepositoryCRUD<Users>().GetByID(model.Id);
            if (user != null)
            {
                if (user.Id != GetCurrentUserId())
                {
                    try
                    {
                        unitOfWork.RepositoryCRUD<Users>().Update(user);
                        user.IsEnabled = false;
                        await unitOfWork.CommitAsync();
                        return JsonUtil.Success(user);
                    }
                    catch (Exception ex)
                    {
                        return JsonUtil.Error(ex.Message);
                    }
                }
                return JsonUtil.Error("Cannot delete current user");
            }
            return JsonUtil.Error("User does not exist");
        }
    }
}