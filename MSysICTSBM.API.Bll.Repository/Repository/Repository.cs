using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MSysICTSBM.API.Bll.Repository.Repository
{
    public class Repository:IRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Repository> _logger;

        public Repository(IConfiguration configuration, ILogger<Repository> logger)
        {
            //dbMain = devICTSBMMainEntities;
            _configuration = configuration;
            _logger = logger;
        }



        public async Task<string> LoginAsync(int AppId)
        {
            try
            {
                using (MSysMainEntities dbMain = new MSysMainEntities())
                {
                    //var UserId = await dbMain.UserInApps.Where(a => a.AppId == AppId).Select(a => a.UserId).FirstOrDefaultAsync();
                    string Email = "Milind@123.com";
                    //var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
                    //if (!string.IsNullOrEmpty(UserId))
                    //{
                    //    Email = await dbMain.AspNetUsers.Where(a => a.Id == UserId).Select(a => a.Email).FirstOrDefaultAsync();
                    //}

                    if (string.IsNullOrEmpty(Email))
                    {
                        return null;
                    }
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,Email),
                         new Claim("AppId",AppId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                    var authSigninkey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(12),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigninkey, SecurityAlgorithms.HmacSha256Signature));
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return "Error Occured";
            }


        }

    }
}
