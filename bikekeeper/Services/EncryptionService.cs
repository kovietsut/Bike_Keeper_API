using bikekeeper.Repository.Abstract;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data.Models;
using Infrastructure.Security;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Services
{
    public class EncryptionService: IEncryptionService
    {
        protected readonly IUnitOfWork unitOfWork;
        private readonly Encryption encryption;
        protected readonly AppSettings appSettings;

        public EncryptionService(IUnitOfWork UnitOfWork, IOptions<AppSettings> AppSettings)
        {
            unitOfWork = UnitOfWork;
            encryption = new Encryption();
            appSettings = AppSettings.Value;
        }

        public string CreateSalt()
        {
            return encryption.CreateSalt();
        }

        public string CreateSalt(string value)
        {
            return encryption.CreateSalt(new object[] { value, appSettings.Salt });
        }

        public string EncryptPassword(string password, string securityStamp)
        {
            return encryption.EncryptPassword(password, securityStamp);
        }

        public string HashSHA256(string value)
        {
            return encryption.HashSHA256(value);
        }
    }
}
