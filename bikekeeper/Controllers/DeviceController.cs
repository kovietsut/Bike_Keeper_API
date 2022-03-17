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
    public class DeviceController : GeneralController<DeviceModel, Device>
    {
        public DeviceController(IGeneralService<DeviceModel, Device> IGeneralService, IUnitOfWork IUnitOfWork) : base(IGeneralService, IUnitOfWork)
        {

        }

        [HttpGet("GetDeviceByPark")]
        public JsonResult GetDeviceByPark(int Id)
        {
            var park = unitOfWork.RepositoryCRUD<Device>().GetAll().Where(x => x.ParkId == Id);
            if(park != null)
            {
                var totalRecords = park.Count();
                return JsonUtil.Success(park, "Get Device By Park Sucessfully", totalRecords);
            }
            return JsonUtil.Error("Empty Data");
        }

        [HttpPost("Create")]
        public override async Task<JsonResult> Create([FromBody] DeviceModel model)
        {
            var checkCodeDevice = unitOfWork.RepositoryCRUD<Device>().Any(x => x.Code == model.Code);
            if (checkCodeDevice)
            {
                return JsonUtil.Error("Code device existed");
            }
            var checkNameDevice = unitOfWork.RepositoryCRUD<Device>().Any(x => x.Name == model.Name);
            if (checkNameDevice)
            {
                return JsonUtil.Error("Name device existed");
            }
            var checkMacAddress = unitOfWork.RepositoryCRUD<Device>().Any(x => x.MacAddress == model.MacAddress);
            if (checkMacAddress)
            {
                return JsonUtil.Error("Mac Address existed");
            }
            var result = await iGeneralService.Create(model);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result);
            }
            return JsonUtil.Error("Create Fail");
        }

        [HttpPost("GetListDevice")]
        public JsonResult GetListDevice([FromBody] SearchListParkModel model)
        {
            var devies = unitOfWork.RepositoryCRUD<Device>().GetAll();
            var parks = unitOfWork.RepositoryCRUD<Park>().GetAll();

            var joinTables = devies
                .Join(parks,
                    device => device.ParkId,
                    park => park.Id,
                    (device, park) =>
                    new
                    {
                        DeviceId = device.Id,
                        DeviceCode = device.Code,
                        DeviceName = device.Name,
                        DeviceStatus = device.Status,
                        MacAddress = device.MacAddress,
                        ParkId = device.ParkId,
                        ParkStatus = park.Status,
                        AvaialableTime = park.AvailableTime,
                        OpenCloseTime = park.OpenCloseTime,
                        ParkName = park.Name
                    }
                );
            var data = joinTables
                .Where(x => (x.ParkId == model.ParkId || model.ParkId == null) && (model.SearchText == "" || x.DeviceCode.Contains(model.SearchText.Trim()) || x.DeviceStatus.Contains(model.SearchText.Trim())  ||
                 x.MacAddress.Contains(model.SearchText.Trim()) || x.ParkStatus.Contains(model.SearchText.Trim()) ||  x.AvaialableTime.ToString().Contains(model.SearchText.Trim()) ||
                 x.OpenCloseTime.ToString().Contains(model.SearchText.Trim()) || x.ParkName.Contains(model.SearchText.Trim())));
            var result = data
            .Skip(((int)model.PageNumber - 1) * (int)model.PageSize)
            .Take((int)model.PageSize);
            if (result != null)
            {
                var totalRecords = data.Count();
                return JsonUtil.Success(result, "Get List Device Successful", totalRecords);
            }
            return JsonUtil.Error("Empty Data");
        }
    }
}
