using bikekeeper.Repository.Abstract;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data.Entity;
using Biker_Keeper_Data.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth;
using Infrastructure.Security;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bikekeeper.Services
{
    public class AccountService : IAccountService
    {
        protected readonly IUnitOfWork unitOfWork;
        private readonly JwtConfiguration jwtOptions;
        private readonly IEncryptionService iEncryptionService;

        public AccountService(IUnitOfWork UnitOfWork, IOptions<JwtConfiguration> JwtOptions, IEncryptionService IEncryptionService)
        {
            unitOfWork = UnitOfWork;
            jwtOptions = JwtOptions.Value;
            iEncryptionService = IEncryptionService;
        }

        public async Task<dynamic> SignIn(UserModel model)
        {
            var user = unitOfWork.RepositoryCRUD<Users>().GetSingleWithNoIsEnable(x => x.Username == model.Username);
            if (user == null)
            {
                return JsonUtil.Error("Incorrect username or password");
            }
            if (user.IsEnabled == false)
            {
                return JsonUtil.Error("Your account is blocked");
            }
            var checkPass = iEncryptionService.EncryptPassword(model.Password, user.SecurityStamp);
            if (user.PasswordHash != checkPass)
            {
                return JsonUtil.Error("Incorrect username or password");
            }

            var userRoles = unitOfWork.RepositoryCRUD<UsersRoles>().GetAll().Where(x => x.UserId == user.Id);
            Dictionary<string, string> userRoleIds = new Dictionary<string, string>();
            foreach (var userRole in userRoles)
            {
                var role = unitOfWork.RepositoryCRUD<Roles>().GetSingle(x => x.Id == userRole.RoleId);
                userRoleIds.Add(role.Name, userRole.RoleId.ToString());
            }

            var company = unitOfWork.RepositoryCRUD<Company>().GetSingle(x => x.Id == user.CompanyId);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.CHash, iEncryptionService.HashSHA256(user.SecurityStamp)),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore: jwtOptions.NotBefore,
                expires: jwtOptions.Expiration,
                signingCredentials: jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            return JsonUtil.Success(new
            {
                account = new UserData
                {
                    UserId = user.Id.ToString(),
                    UserName = user.Username,
                    UserFullName = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Expires = (int)jwtOptions.ValidFor.TotalSeconds,
                    ExpiresDate = DateTime.Now.AddDays(jwtOptions.ValidFor.Days),
                    Uid = user.Username,
                    Gender = null,
                    Status = null,
                    Address = null
                },
                Roles = userRoleIds,
                token = encodedJwt,
                company = company
            });
        }

        public async Task<dynamic> SignInWithGoogle(UserModel model)
        {
            var user = unitOfWork.RepositoryCRUD<Users>().GetSingleWithNoIsEnable(x => x.Username == model.Email);
            if (user == null)
            {
                return JsonUtil.Error("Incorrect username or password");
            }
            if (user.IsEnabled == false)
            {
                return JsonUtil.Error("Your account is blocked");
            }
            // Login with TokenId
            //var settings = new GoogleJsonWebSignature.ValidationSettings() { Audience = new List<string>() { "156196961698-02eadqj76t9sb5q9dq22k0fc5m825tpq.apps.googleusercontent.com" } };
            //var validPayload = await GoogleJsonWebSignature.ValidateAsync(model.TokenId, settings);
            //var subject = validPayload.Subject;
            // Jwt
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                //new Claim(JwtRegisteredClaimNames.CHash, iEncryptionService.HashSHA256(user.SecurityStamp)),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore: jwtOptions.NotBefore,
                expires: jwtOptions.Expiration,
                signingCredentials: jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var userRoles = unitOfWork.RepositoryCRUD<UsersRoles>().GetAll().Where(x => x.UserId == user.Id);
            Dictionary<string, string> userRoleIds = new Dictionary<string, string>();
            foreach (var userRole in userRoles)
            {
                var role = unitOfWork.RepositoryCRUD<Roles>().GetSingle(x => x.Id == userRole.RoleId);
                userRoleIds.Add(role.Name, userRole.RoleId.ToString());
            }

            var company = unitOfWork.RepositoryCRUD<Company>().GetSingle(x => x.Id == user.CompanyId);
            // Serialize and return the response
            return JsonUtil.Success(new
            {
                account = new UserData
                {
                    //UserId = user.Id.ToString(),
                    //UserName = validPayload.Email,
                    //UserFullName = validPayload.GivenName + " " + validPayload.FamilyName,
                    //Email = validPayload.Email,
                    UserName = user.Email,
                    UserFullName = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Expires = (int)jwtOptions.ValidFor.TotalSeconds,
                    ExpiresDate = DateTime.Now.AddDays(jwtOptions.ValidFor.Days),
                    Uid = user.Username,
                    Gender = null,
                    Status = null,
                    Address = null,
                    DateOfBirth = user.DateOfBirth
                },
                Roles = userRoleIds,
                token = encodedJwt,
                //tokenId = model.TokenId,
                //accessToken = model.AccessToken,
                company = company
            });
        }

        public JsonResult UpdateUser(UserModel model)
        {
            var user = unitOfWork.RepositoryCRUD<Users>().GetSingle(x => x.Id == model.Id);
            if (user == null)
            {
                return JsonUtil.Error("Username does not exist");
            }
            user.Code = model.Code;
            user.Name = model.Name;
            user.Username = model.Username;
            var isNumber = Regex.IsMatch(model.PhoneNumber, "^[0-9]*$");
            if (model.PhoneNumber.Length != 10 || isNumber == false)
            {
                return JsonUtil.Error("Incorect format phone number");
            }
            if (model.PhoneNumber == null) return JsonUtil.Error("Phone number cannot be empty");
            else
            {
                var checkPhoneNumber = unitOfWork.RepositoryCRUD<Users>().Any(x => x.PhoneNumber == model.PhoneNumber && x.PhoneNumber != user.PhoneNumber);
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
                var checkEmail = unitOfWork.RepositoryCRUD<Users>().Any(x => x.Email == model.Email && x.Email != user.Email);
                if (checkEmail)
                {
                    return JsonUtil.Error("Email existed");
                }
                user.Email = model.Email;
            }
            user.DateOfBirth = model.DateOfBirth;
            user.Status = "Active";
            user.ModifiedWhen = DateTime.Now;
            user.CompanyId = model.CompanyId;
            unitOfWork.RepositoryCRUD<Users>().Update(user);
            unitOfWork.Commit();
            if (model.RoleIds != null)
            {
                unitOfWork.RepositoryCRUD<UsersRoles>().DeleteWhere(x => x.UserId == user.Id);
                var listRole = model.RoleIds.Split(',');
                foreach (string roleIdString in listRole)
                {
                    int roleId = Int32.Parse(roleIdString);
                    UsersRoles userRole = new UsersRoles();
                    userRole.UserId = model.Id;
                    userRole.RoleId = roleId;
                    unitOfWork.RepositoryCRUD<UsersRoles>().Insert(userRole);
                    unitOfWork.Commit();
                }
            }
            return JsonUtil.Success(user);
        }

    }
}
