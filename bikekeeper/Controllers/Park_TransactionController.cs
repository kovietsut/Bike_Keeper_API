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
using bikekeeper.Services.Abstract;
using bikekeeper.Repository.Abstract;
using Infrastructure.Utils;
using Infrastructure;

namespace bikekeeper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Park_TransactionController : GeneralController<TransactionModel, Park_Transaction>
    {
        public Park_TransactionController(IGeneralService<TransactionModel, Park_Transaction> IGeneralService, IUnitOfWork IUnitOfWork) : base(IGeneralService, IUnitOfWork)
        {

        }

        [HttpPost("GetListTransaction")]
        public JsonResult GetListTransaction([FromBody] SearchListUserModel model)
        {
            var transactions = unitOfWork.RepositoryCRUD<Park_Transaction>().GetAll();
            var companies = unitOfWork.RepositoryCRUD<Company>().GetAll();
            var cards = unitOfWork.RepositoryCRUD<Parking_Card>().GetAll();
            var vehicleTypes = unitOfWork.RepositoryCRUD<VehicleType>().GetAll();
            var devices = unitOfWork.RepositoryCRUD<Device>().GetAll();
            var users = unitOfWork.RepositoryCRUD<Users>().GetAll();
            var parks = unitOfWork.RepositoryCRUD<Park>().GetAll();

            var joinTables = transactions
                .Join(cards,
                    transaction => transaction.ParkingCardId,
                    card => card.Id,
                    (transaction, card) =>
                    new
                    {
                        Id = transaction.Id,
                        CheckTime = transaction.CheckTime,
                        VehicleTypeId = transaction.VehicleTypeId,
                        DeviceId = transaction.DeviceId,
                        UserId = transaction.UserId,
                        CardId = card.Id,
                        QRCode = card.QRCode,
                        ParkId = card.ParkId
                    })
                .Join(vehicleTypes,
                    transaction => transaction.VehicleTypeId,
                    vehicleType => vehicleType.Id,
                    (transaction, vehicleType) =>
                    new
                    {
                        Id = transaction.Id,
                        CheckTime = transaction.CheckTime,
                        VehicleTypeId = transaction.VehicleTypeId,
                        DeviceId = transaction.DeviceId,
                        UserId = transaction.UserId,
                        CardId = transaction.Id,
                        QRCode = transaction.QRCode,
                        ParkId = transaction.ParkId,
                        BrandName = vehicleType.BrandName
                    })
                .Join(devices,
                    transaction => transaction.DeviceId,
                    device => device.Id,
                    (transaction, device) =>
                    new
                    {
                        Id = transaction.Id,
                        CheckTime = transaction.CheckTime,
                        VehicleTypeId = transaction.VehicleTypeId,
                        DeviceId = transaction.DeviceId,
                        UserId = transaction.UserId,
                        CardId = transaction.Id,
                        QRCode = transaction.QRCode,
                        ParkId = transaction.ParkId,
                        BrandName = transaction.BrandName,
                        MacAddress = device.MacAddress
                    })
                .Join(users,
                    transaction => transaction.UserId,
                    user => user.Id,
                    (transaction, user) =>
                    new
                    {
                        Id = transaction.Id,
                        CheckTime = transaction.CheckTime,
                        VehicleTypeId = transaction.VehicleTypeId,
                        DeviceId = transaction.DeviceId,
                        UserId = transaction.UserId,
                        CardId = transaction.Id,
                        QRCode = transaction.QRCode,
                        ParkId = transaction.ParkId,
                        BrandName = transaction.BrandName,
                        MacAddress = transaction.MacAddress,
                        FullName = user.Name,
                        PhoneNumber = user.PhoneNumber
                    })
                .Join(parks,
                    card => card.ParkId,
                    park => park.Id,
                    (card, park) =>
                    new
                    {
                        Id = card.Id,
                        CheckTime = card.CheckTime,
                        VehicleTypeId = card.VehicleTypeId,
                        DeviceId = card.DeviceId,
                        UserId = card.UserId,
                        CardId = card.Id,
                        QRCode = card.QRCode,
                        ParkId = card.ParkId,
                        BrandName = card.BrandName,
                        MacAddress = card.MacAddress,
                        PhoneNumber = card.PhoneNumber,
                        ParkName = park.Name,
                        CompanyId = park.CompanyId
                    })
                .Join(companies,
                    park => park.CompanyId,
                    company => company.Id,
                    (park, company) =>
                    new
                    {
                        Id = park.Id,
                        CheckTime = park.CheckTime,
                        VehicleTypeId = park.VehicleTypeId,
                        DeviceId = park.DeviceId,
                        UserId = park.UserId,
                        CardId = park.Id,
                        QRCode = park.QRCode,
                        ParkId = park.ParkId,
                        BrandName = park.BrandName,
                        MacAddress = park.MacAddress,
                        PhoneNumber = park.PhoneNumber,
                        ParkName = park.ParkName,
                        CompanyId = park.CompanyId,
                        CompanyLocation = company.Location
                    });
            // Better performance, return object instead of array
            var data = joinTables //ImageVehicle
                .Where(x => model.SearchText == "" || x.CheckTime.ToString().Contains(model.SearchText.Trim()) ||
                    x.CompanyLocation.Contains(model.SearchText.Trim()) || x.QRCode.Contains(model.SearchText.Trim()) ||
                    x.BrandName.Contains(model.SearchText.Trim()) || x.MacAddress.Contains(model.SearchText.Trim()) ||
                    x.ParkName.Contains(model.SearchText.Trim()) ||
                    x.PhoneNumber.Contains(model.SearchText.Trim()));

            var result = data
            .Skip(((int)model.PageNumber - 1) * (int)model.PageSize)
            .Take((int)model.PageSize);
            if (result != null)
            {
                var totalRecords = data.Count();
                return JsonUtil.Success(result, "Get List Transaction Successful", totalRecords);
            }
            return JsonUtil.Error("Empty Data");
        }


        [HttpPost("CheckQR")]
        public async Task<JsonResult> CheckQR([FromBody] CheckQRModel model)
        {
            var isExist = unitOfWork.RepositoryCRUD<Parking_Card>().Any(x => x.QRCode == model.QRCode);
            if (!isExist)
            {
                return JsonUtil.Error("QRCode not exist");
            }
            var parkingCard = unitOfWork.RepositoryCRUD<Parking_Card>().GetSingle(x => x.QRCode == model.QRCode);
            var transactions = unitOfWork.RepositoryCRUD<Park_Transaction>().GetAll().Where(x => x.ParkingCardId == parkingCard.Id);
            var totalCheckIn = transactions.Where(x => x.TransactionType == "CheckIn").Count();
            var totalCheckOut = transactions.Where(x => x.TransactionType == "CheckOut").Count();

            if (totalCheckIn == totalCheckOut)
            {
                return JsonUtil.Success(new CheckQR { TransactionType = "CheckIn" });
            }
            else if (totalCheckIn > totalCheckOut)
            {
                return JsonUtil.Success(new CheckQR { TransactionType = "CheckOut" });
            }
            return JsonUtil.Error("Cannot get Transaction Type");
        }

        [HttpPost("GetTransactionHistory")]
        public async Task<JsonResult> History()
        {
            var userId = GetCurrentUserId();
            var transactions = unitOfWork.RepositoryCRUD<Park_Transaction>().GetAll().Where(x => x.UserId == userId);
            return JsonUtil.Success(transactions);
        }


        [HttpPost("Create")]
        public override async Task<JsonResult> Create([FromBody] TransactionModel model)
        {
            var result = new ResponseModel();
            if (model.TransactionType == "CheckIn")
            {
                //var park = unitOfWork.RepositoryCRUD<Park>().GetSingle(x => x.Name == "FPT HCM");
                var isExist = unitOfWork.RepositoryCRUD<Parking_Card>().Any(x => x.QRCode == model.QRCode);
                if (!isExist)
                {
                    return JsonUtil.Error("QRCode not exist");
                }
                var parkingCard = unitOfWork.RepositoryCRUD<Parking_Card>().GetSingle(x => x.QRCode == model.QRCode);
                var park = unitOfWork.RepositoryCRUD<Park>().GetSingle(x => x.Id == parkingCard.ParkId);
                var vehicleType = unitOfWork.RepositoryCRUD<VehicleType>().GetSingle(x => x.QRCodeId == parkingCard.Id);
                if (vehicleType == null)
                {
                    vehicleType = new VehicleType();
                    vehicleType.BrandName = model.VehicleManufacturer;
                    vehicleType.LicensePlate = model.LicensePlate;
                    vehicleType.QRCodeId = parkingCard.Id;
                    vehicleType.ImageVehicle = model.ImageVehicle;
                    vehicleType.IsCheckIn = true;
                    unitOfWork.RepositoryCRUD<VehicleType>().Insert(vehicleType);
                    unitOfWork.Commit();
                }
                else
                {
                    if (vehicleType.LicensePlate != model.LicensePlate)
                    {
                        return JsonUtil.Error("LicensePlate not match QrCode");
                    }
                    vehicleType.IsCheckIn = !vehicleType.IsCheckIn;
                    unitOfWork.RepositoryCRUD<VehicleType>().Update(vehicleType);
                }
                model.TransactionType = "CheckOut";
                if (vehicleType.IsCheckIn)
                {
                    model.TransactionType = "CheckIn";
                }
                model.ParkingCardId = parkingCard.Id;
                model.VehicleTypeId = vehicleType.Id;
                model.DeviceId = 1;
                model.CheckTime = DateTime.Now;
                model.UserId = GetCurrentUserId();
                

                result = await iGeneralService.Create(model);

                return JsonUtil.Success(new CheckIn
                {
                    Message = model.TransactionType+" Success",
                    CheckTime = model.CheckTime,
                });

                //var parkKindVehicle = unitOfWork.RepositoryCRUD<ParkKindVehicle>().GetSingle(x => x.ParkId == park.Id && x.Status == "Empty");
                //if (parkKindVehicle.Status == "Empty")
                //{
                //    parkKindVehicle.Status = "Active";
                //    unitOfWork.RepositoryCRUD<ParkKindVehicle>().Update(parkKindVehicle);
                //    unitOfWork.Commit();
                //    return JsonUtil.Success(new CheckIn
                //    {
                //        Message = "Check In Success",
                //        CheckTime = model.CheckTime,
                //        //Position = "Location suggest: " + parkKindVehicle.LocationKindVehicle
                //    });
                //}
                //else
                //{
                //    return JsonUtil.Error("Position at QRCode is active");
                //}
            }
            /*if (model.TransactionType == "CheckOut")
            {
                //var park = unitOfWork.RepositoryCRUD<Park>().GetSingle(x => x.Name == "FPT HCM");
                var isExist = unitOfWork.RepositoryCRUD<Parking_Card>().Any(x => x.QRCode == model.QRCode);
                if (!isExist)
                {
                    JsonUtil.Error("QRCode not exist");
                }

                var parkingCard = unitOfWork.RepositoryCRUD<Parking_Card>().GetSingle(x => x.QRCode == model.QRCode);
                var park = unitOfWork.RepositoryCRUD<Park>().GetSingle(x => x.Id == parkingCard.ParkId);

                model.ParkingCardId = parkingCard.Id;
                model.CheckTime = DateTime.Now;
                model.UserId = GetCurrentUserId();
                //var parkTransaction = unitOfWork.RepositoryCRUD<Park_Transaction>().GetSingle(x => x.)
                var transactions = unitOfWork.RepositoryCRUD<Park_Transaction>().GetAll().Where(x => x.ParkingCardId == parkingCard.Id);
                var checkIn = transactions.Where(x => x.TransactionType == "CheckIn");

                var totalCheckIn = checkIn.Count();
                var totalCheckOut = transactions.Where(x => x.TransactionType == "CheckOut").Count();
                if (totalCheckOut < totalCheckIn)
                {
                    result = await iGeneralService.Create(model);
                }
                else
                {
                    return JsonUtil.Error("Vehicle is checked out");
                }


                var isValid = checkIn.OrderBy(x => x.CheckTime).Last().LicensePlate == model.LicensePlate;
                if (isValid)
                {
                    return JsonUtil.Success(new CheckOut
                    {
                        Message = "Check out Success",
                        CheckTime = model.CheckTime,
                        Price = 3000
                    });
                } else
                {
                    return JsonUtil.Error("Wrong License Plate");
                }
                //var parkKindVehicle = unitOfWork.RepositoryCRUD<ParkKindVehicle>().GetSingle(x => x.ParkId == park.Id && x.Status == "Active");
                //if (parkKindVehicle.Status == "Active")
                //{
                //    parkKindVehicle.Status = "Empty";
                //    unitOfWork.RepositoryCRUD<ParkKindVehicle>().Update(parkKindVehicle);
                //    unitOfWork.Commit();
                //    return JsonUtil.Success(new CheckOut
                //    {
                //        Message = "Check out Success",
                //        CheckTime = model.CheckTime,
                //        Price = 3000
                //    });
                //}
            }*/
            return JsonUtil.Error("Cannot Create Transaction");
        }
    }
}
