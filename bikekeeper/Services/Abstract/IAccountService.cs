using Biker_Keeper_Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Services.Abstract
{
    public interface  IAccountService
    {
        Task<dynamic> SignIn(UserModel model);
        Task<dynamic> SignInWithGoogle(UserModel model);
        JsonResult UpdateUser(UserModel model);
    }
}
