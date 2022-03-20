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
using Biker_Keeper_Data.Models;
using bikekeeper.Repository.Abstract;
using Infrastructure.Utils;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Parking_CardController : GeneralController<ParkingCardModel, Parking_Card>
    {
        public Parking_CardController(IGeneralService<ParkingCardModel, Parking_Card> IGeneralService, IUnitOfWork IUnitOfWork) : base(IGeneralService, IUnitOfWork)
        {

        }

        [HttpGet("GetCardDetail")]
        public JsonResult GetCardDetail(int id)
        {
            var card = unitOfWork.RepositoryCRUD<Parking_Card>().GetByID(id);
            if(card == null)
            {
                return JsonUtil.Error("Card does not exist");
            }
            var park = unitOfWork.RepositoryCRUD<Park>().GetByID((int)card.ParkId);
            if(park == null)
            {
                return JsonUtil.Error("Park does not exist");
            }
            var company = unitOfWork.RepositoryCRUD<Company>().GetByID((int)park.CompanyId);
            if (company == null)
            {
                return JsonUtil.Error("Company does not exist");
            }
            var parkKindVehicle = unitOfWork.RepositoryCRUD<ParkKindVehicle>().GetByID((int)park.ParkKindVehicleId);
            if (parkKindVehicle == null)
            {
                return JsonUtil.Error("Park kind vehicle does not exist");
            }
            return JsonUtil.Success(new
            {
                Id = card.Id,
                QRCode = card.QRCode,
                IsEnable = card.IsEnabled,
                ParkId = card.ParkId,
                CompanyId = company.Id,
                ParkKindVehicleId = parkKindVehicle.Id
            });
        }

        [HttpGet("GetCardByPark")]
        public JsonResult GetCardByPark(int Id)
        {
            var card = unitOfWork.RepositoryCRUD<Parking_Card>().GetAll().Where(x => x.ParkId == Id);
            if (card != null)
            {
                var totalRecords = card.Count();
                return JsonUtil.Success(card, "Get Card By Park Sucessfully", totalRecords);
            }
            return JsonUtil.Error("Empty Data");
        }

        [HttpPost("Create")]
        public override async Task<JsonResult> Create([FromBody] ParkingCardModel model)
        {
            var checkQRCode = unitOfWork.RepositoryCRUD<Parking_Card>().Any(x => x.QRCode == model.QRCode);
            if (checkQRCode)
            {
                return JsonUtil.Error("QRCode existed");
            }
            var result = await iGeneralService.Create(model);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result);
            }
            return JsonUtil.Error("Create Fail");
        }

        [HttpPost("GetListCard")]
        public JsonResult GetListCard([FromBody] SearchListCardModel model)
        {
            var cards = unitOfWork.RepositoryCRUD<Parking_Card>().GetAll();
            var parks = unitOfWork.RepositoryCRUD<Park>().GetAll();
            var kindVehicles = unitOfWork.RepositoryCRUD<ParkKindVehicle>().GetAll();
            var companies = unitOfWork.RepositoryCRUD<Company>().GetAll();

            var joinTables = cards
                .Join(parks,
                    card => card.ParkId,
                    park => park.Id,
                    (card, park) =>
                    new
                    {
                        Id = card.Id,
                        QRCode = card.QRCode,
                        ParkId = card.ParkId,
                        ParkCode = park.Code,
                        ParkName = park.Name,
                        CompanyId = park.CompanyId,
                        ParkStatus = park.Status,
                        AvailableTime = park.AvailableTime,
                        OpenCloseTime = park.OpenCloseTime,
                        ParkKindVehicleId = park.ParkKindVehicleId
                    })
                .Join(kindVehicles,
                    card => card.ParkKindVehicleId,
                    kindVehicle => kindVehicle.Id,
                    (card, kindVehicle) =>
                    new
                    {
                        Id = card.Id,
                        QRCode = card.QRCode,
                        ParkId = card.ParkId,
                        ParkCode = card.ParkCode,
                        ParkName = card.ParkName,
                        CompanyId = card.CompanyId,
                        ParkStatus = card.ParkStatus,
                        AvailableTime = card.AvailableTime,
                        OpenCloseTime = card.OpenCloseTime,
                        ParkKindVehicleId = card.ParkKindVehicleId,
                        KindVehicleCode = kindVehicle.Code,
                        LocationKindVehicle = kindVehicle.LocationKindVehicle
                    })
                .Join(companies,
                    card => card.CompanyId,
                    company => company.Id,
                    (card, company) =>
                    new
                    {
                        Id = card.Id,
                        QRCode = card.QRCode,
                        ParkId = card.ParkId,
                        ParkCode = card.ParkCode,
                        ParkName = card.ParkName,
                        CompanyId = card.CompanyId,
                        ParkStatus = card.ParkStatus,
                        AvailableTime = card.AvailableTime,
                        OpenCloseTime = card.OpenCloseTime,
                        ParkKindVehicleId = card.ParkKindVehicleId,
                        KindVehicleCode = card.KindVehicleCode,
                        LocationKindVehicle = card.LocationKindVehicle,
                        CompanyCode = company.Code,
                        CompanyName = company.Name,
                        CompanyLocation = company.Location
                    });

            var data = joinTables
                .Where(x => (x.ParkId == model.ParkId || model.ParkId == null) && (x.CompanyId == model.CompanyId || model.CompanyId == null) &&
                (x.ParkKindVehicleId == model.ParkKindVehicleId || model.ParkKindVehicleId == null) &&
                (model.SearchText == "" || x.QRCode.Contains(model.SearchText.Trim()) || x.ParkCode.Contains(model.SearchText.Trim()) ||
                 x.ParkStatus.Contains(model.SearchText.Trim()) || x.AvailableTime.ToString().Contains(model.SearchText.Trim()) ||
                 x.OpenCloseTime.ToString().Contains(model.SearchText.Trim()) || x.ParkName.Contains(model.SearchText.Trim()) ||
                 x.KindVehicleCode.Contains(model.SearchText.Trim()) || x.LocationKindVehicle.Contains(model.SearchText.Trim()) || x.CompanyCode.Contains(model.SearchText.Trim()) ||
                 x.CompanyName.Contains(model.SearchText.Trim()) || x.CompanyLocation.Contains(model.SearchText.Trim())));
            var result = data
            .Skip(((int)model.PageNumber - 1) * (int)model.PageSize)
            .Take((int)model.PageSize);
            if (result != null)
            {
                var totalRecords = data.Count();
                return JsonUtil.Success(result, "Get List Card Successful", totalRecords);
            }
            return JsonUtil.Error("Empty Data");
        }
    }
}
