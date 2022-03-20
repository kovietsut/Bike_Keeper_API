using AutoMapper;
using bikekeeper.Models;
using bikekeeper.Repository;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using Biker_Keeper_Data.Models;
using Infrastructure.Security;
using System;
using System.Linq;

namespace bikekeeper.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // DomainToModelMappingProfile
            CreateMap<Company, CompanyModel>().ReverseMap();
            CreateMap<Park, ParkModel>().ReverseMap();
            CreateMap<Pages, PageFilterModel>().ReverseMap();
            CreateMap<Device, DeviceModel>().ReverseMap();
            CreateMap<ParkKindVehicle, ParkKindVehicleModel>().ReverseMap();
            CreateMap<VehicleType, VehicleTypeModel>().ReverseMap();
            CreateMap<Park_Transaction, TransactionModel>().ReverseMap();
            CreateMap<Parking_Card, ParkingCardModel>().ReverseMap();
            // ModelToDomainMappingProfile
            CreateMap<UserModel, Users>()
                .AfterMap((src, dest) =>
                {
                    if (src.IsGoogleAccount == "off")
                    {
                        SetGeneralColsCreate(dest);
                        dest.SecurityStamp = Guid.NewGuid().ToString();
                        dest.PasswordHash = new Encryption().EncryptPassword(src.Password, dest.SecurityStamp);
                    }
                    if (src.IsGoogleAccount == "on")
                    {
                        dest.SecurityStamp = Guid.NewGuid().ToString();
                    }
                }).ReverseMap();            
        }

        public void SetGeneralColsCreate(IEntityBase data)
        {
            data.Id = 0;
            data.IsEnabled = true;
        }
    }
}
