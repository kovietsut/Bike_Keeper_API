using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Services.Abstract
{
    public interface IEncryptionService
    {
		string CreateSalt();
        string CreateSalt(string code);
        string EncryptPassword(string userName, string passWord);
		string HashSHA256(string value);
    }
}
