using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MSysICTSBM.API.Bll.ViewModels.Models;
using MSysICTSBM.Dal.DataContexts.Models.DB;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainModels;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainSPModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MSysICTSBM.API.Bll.Repository.Repository
{
    public class Repository:IRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Repository> _logger;
        private readonly MSysMainDb dbMain;
        public Repository(IConfiguration configuration, ILogger<Repository> logger, MSysMainDb dbMain)
        {
            this.dbMain = dbMain;
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



        public async Task<SBUser> CheckUserLoginAsync(SBUser obj)
        {
            SBUser user = new SBUser();
            user = await CheckUserLoginForNormalAsync(obj);
            
            return user;
        }
         
        public async Task<SBUser> CheckUserLoginForNormalAsync(SBUser obj)
        {
            SBUser user = new SBUser();
            try 
            {
                using (dbMain)
                {
                    var userobj = await dbMain.EmployeeMasters.Where(a => a.Username == obj.userLoginId && a.Password == obj.userPassword && a.IsActive == true).FirstOrDefaultAsync();

                    if (userobj == null)
                    {
                        user.userId = 0;
                        user.userLoginId = "";
                        user.userPassword = "";
                        user.status = "error";
                        user.gtFeatures = false;
                        user.imiNo = "";
                        user.EmpType = "";
                        user.message = "UserName or Passward not Match.";
                        user.messageMar = "वापरकर्ता नाव किंवा पासवर्ड जुळत नाही.";
                        user.token = "";
                    }
                    else
                    {

                        user.type = userobj.Type;
                        user.typeId = 0;
                        user.userId = userobj.Id;
                        user.userLoginId = userobj.Username;
                        user.userPassword = "";
                        user.imiNo = "";
                        user.EmpType = obj.EmpType;
                        
                        user.status = "success"; 
                        user.message = "Login Successfully"; 
                        user.messageMar = "लॉगिन यशस्वी";
                        user.token = await LoginAsync(userobj.Username, userobj.Type, userobj.Id);

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                user.status = "error";
                user.message = "Something is wrong,Try Again.. ";
                user.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                return user;
            }

            return user;
        }

        public async Task<string> LoginAsync(string userName, string EmpType, int UserId)
        {
            try
            {
                    
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(EmpType))
                    {
                        return null;
                    }
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,userName),
                         new Claim("EmpType",EmpType),
                         new Claim("UserId",UserId.ToString()),
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return "Error Occured";
            }


        }

        public async Task<Result> SaveULBDetailsAsync(ULB_DetailVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var ulbObj = await dbMain.ULB_Details.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.CreateUserid))
                    {
                        if (ulbObj != null)
                        {
                            ulbObj.AppID = obj.AppID;
                            ulbObj.AppName = obj.AppName;
                            ulbObj.House_property = obj.House_property;
                            ulbObj.Dump_property = obj.Dump_property;
                            ulbObj.Liquid_property = obj.Liquid_property;
                            ulbObj.Street_property = obj.Street_property;
                            ulbObj.IsActive = obj.IsActive;
                            ulbObj.UpdateUserid = obj.CreateUserid;
                            ulbObj.UpdateDate = DateTime.Now;
                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "ULB Details Updated Successfully";
                            result.messageMar = "ULB तपशील यशस्वीरित्या बदलले";

                        }
                        else
                        {
                            var ulbObjData = new ULB_Detail();
                            ulbObjData.AppID = obj.AppID;
                            ulbObjData.AppName = obj.AppName;
                            ulbObjData.House_property = obj.House_property;
                            ulbObjData.Dump_property = obj.Dump_property;
                            ulbObjData.Liquid_property = obj.Liquid_property;
                            ulbObjData.Street_property = obj.Street_property;
                            ulbObjData.IsActive = obj.IsActive;
                            ulbObjData.CreateUserid = obj.CreateUserid;
                            ulbObjData.CreateDate = DateTime.Now;
                            ulbObjData.UpdateDate = DateTime.Now;

                            dbMain.ULB_Details.Add(ulbObjData);
                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "ULB Details Added Successfully";
                            result.messageMar = "ULB तपशील यशस्वीरित्या समाविष्ट केले";

                        }
                    }
                    else
                    {
                        result.status = "Error";
                        result.message = "User Name  not Exist";
                        result.messageMar = "वापरकर्ता नाव अस्तित्वात नाही..";

                    }
                    
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                result.status = "Error";
                result.message = "Something is wrong,Try Again.. ";
                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                return result;
            }


        }


        public async Task<Result> SaveUserAsync(EmployeeMasterVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var userObj = await dbMain.EmployeeMasters.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
                    if (userObj != null)
                    {
                        if (await dbMain.EmployeeMasters.AnyAsync(a => a.Username == obj.Username && a.Id != obj.Id))
                        {


                            result.status = "Error";
                            result.message = "User Name  already Exist";
                            result.messageMar = "नाव आधीपासून अस्तित्वात आहे..";
                        }
                        else
                        {
                            userObj.Name = obj.Name;
                            userObj.Username = obj.Username;
                            userObj.Password = obj.Password;
                            userObj.MobileNumber = obj.MobileNumber;
                            userObj.Address = obj.Address;
                            userObj.Type = obj.Type;
                            userObj.IsActive = obj.IsActive;
                            userObj.IsActiveULB = obj.IsActiveULB;
                            userObj.lastModifyDateEntry = DateTime.Now;

                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "User Details Updated Successfully";
                            result.messageMar = "वापरकर्ता तपशील यशस्वीरित्या बदलले";

                        }
                        

                    }
                    else
                    {
                        if (await dbMain.EmployeeMasters.AnyAsync(a => a.Username == obj.Username)) {
                            

                            result.status = "Error";
                            result.message = "User Name  already Exist";
                            result.messageMar = "नाव आधीपासून अस्तित्वात आहे..";
                        }
                        else
                        {
                            var userObjData = new EmployeeMaster();
                            userObjData.Name = obj.Name;
                            userObjData.Username = obj.Username;
                            userObjData.Password = obj.Password;
                            userObjData.MobileNumber = obj.MobileNumber;
                            userObjData.Address = obj.Address;
                            userObjData.Type = obj.Type;
                            userObjData.IsActive = obj.IsActive;
                            userObjData.IsActiveULB = obj.IsActiveULB;
                            userObjData.Create_Date = DateTime.Now;

                            dbMain.EmployeeMasters.Add(userObjData);
                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "User Details Added Successfully";
                            result.messageMar = "वापरकर्ता तपशील यशस्वीरित्या समाविष्ट केले";

                        }

                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                result.status = "Error";
                result.message = "Something is wrong,Try Again.. ";
                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                return result;
            }


        }

        public async Task<List<ULB_DetailVM>> GetAllULBDetailsAsync(int userId)
        {
            List<ULB_DetailVM> result = new List<ULB_DetailVM>();
            try
            {
                using (dbMain)
                {
                    var userobj = await dbMain.EmployeeMasters.Where(a => a.Id == userId && a.IsActive == true).FirstOrDefaultAsync();
                    if (userobj != null)
                    {
                        if (userobj.Type != null && (userobj.Type.ToUpper() == "S" || userobj.Type.ToUpper() == "A"))
                        {
                            if (userobj.IsActiveULB != null)
                            {
                                string[] arrAppId = userobj.IsActiveULB.Split(',');
                                result = await dbMain.ULB_Details.Where(a => arrAppId.Contains(a.Id.ToString())).Select(a => new ULB_DetailVM
                                {
                                    Id = a.Id,
                                    AppID = a.AppID,
                                    AppName = a.AppName,
                                    House_property = a.House_property,
                                    Dump_property = a.Dump_property,
                                    Liquid_property = a.Liquid_property,
                                    Street_property = a.Street_property,
                                    IsActive = a.IsActive,
                                    CreateUserid = a.CreateUserid,
                                    CreateDate = a.CreateDate,
                                    UpdateUserid = a.UpdateUserid,
                                    UpdateDate = a.UpdateDate,
                                    CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.CreateUserid).Select(e => e.Username).FirstOrDefault(),
                                    UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserid).Select(e => e.Username).FirstOrDefault()

                                }).ToListAsync();
                            }
                            
                            
                        }
                        else if (userobj.Type != null && userobj.Type.ToUpper() == "SA")
                        {
                            result = await dbMain.ULB_Details.Select(a => new ULB_DetailVM
                            {
                                Id = a.Id,
                                AppID = a.AppID,
                                AppName = a.AppName,
                                House_property = a.House_property,
                                Dump_property = a.Dump_property,
                                Liquid_property = a.Liquid_property,
                                Street_property = a.Street_property,
                                IsActive = a.IsActive,
                                CreateUserid = a.CreateUserid,
                                CreateDate = a.CreateDate,
                                UpdateUserid = a.UpdateUserid,
                                UpdateDate = a.UpdateDate,
                                CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.CreateUserid).Select(e => e.Username).FirstOrDefault(),
                                UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserid).Select(e => e.Username).FirstOrDefault()

                            }).ToListAsync();
                        }
                    }
                    
                }
                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.IsOldULB = IsOldUlb(item.CreateDate);
                    }

                }
               
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);

                return result;

            }
            
       }

        public async Task<ULB_DetailVM> GetULBDetailsAsync(int Id)
        {
            ULB_DetailVM result = new ULB_DetailVM();
            try
            {
                using (dbMain)
                {
                    result = await dbMain.ULB_Details.Where(a => a.Id == Id).Select(a => new ULB_DetailVM
                    {
                        Id = a.Id,
                        AppID = a.AppID,
                        AppName = a.AppName,
                        House_property = a.House_property,
                        Dump_property = a.Dump_property,
                        Liquid_property = a.Liquid_property,
                        Street_property = a.Street_property,
                        IsActive = a.IsActive,
                        CreateUserid = a.CreateUserid,
                        CreateDate = a.CreateDate,
                        UpdateUserid = a.UpdateUserid,
                        UpdateDate = a.UpdateDate,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.CreateUserid).Select(e => e.Username).FirstOrDefault(),
                        UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserid).Select(e => e.Username).FirstOrDefault()
                    }).FirstOrDefaultAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }


        public async Task<Result> SaveQrPrintAsync(QrPrintedVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var printObj = await dbMain.QrPrinteds.Where(a => a.PrintId == obj.PrintId).FirstOrDefaultAsync();

                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.UserId))
                    {
                        if (printObj != null)
                        {
                            printObj.ULBId = obj.ULBId;
                            printObj.HouseQty = obj.HouseQty;
                            printObj.HouseGreen = obj.HouseGreen;
                            printObj.HouseBlue = obj.HouseBlue;
                            printObj.BannerAcrylic = obj.BannerAcrylic;
                            printObj.DumpAcrylic = obj.DumpAcrylic;
                            printObj.AbhiprayForm = obj.AbhiprayForm;
                            printObj.DisclaimerForm = obj.DisclaimerForm;
                            printObj.DataEntryBook = obj.DataEntryBook;
                            printObj.DumpQty = obj.DumpQty;
                            printObj.StreetQty = obj.StreetQty;
                            printObj.LiquidQty = obj.LiquidQty;
                            printObj.Note = obj.Note;
                            printObj.UserId = obj.UserId;
                            printObj.UpdationDate = DateTime.Now;
                            printObj.UpdateUserId = obj.UserId;

                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "QR Print Details Updated Successfully";
                            result.messageMar = "QR प्रिंट तपशील यशस्वीरित्या बदलले";

                        }
                        else
                        {
                            var printObjData = new QrPrinted();
                            printObjData.ULBId = obj.ULBId;
                            printObjData.PrintDate = obj.PrintDate;
                            printObjData.CreationDate = DateTime.Now;
                            printObjData.UpdationDate = DateTime.Now;
                            printObjData.HouseQty = obj.HouseQty;
                            printObjData.HouseGreen = obj.HouseGreen;
                            printObjData.HouseBlue = obj.HouseBlue;
                            printObjData.BannerAcrylic = obj.BannerAcrylic;
                            printObjData.DumpAcrylic = obj.DumpAcrylic;
                            printObjData.AbhiprayForm = obj.AbhiprayForm;
                            printObjData.DisclaimerForm = obj.DisclaimerForm;
                            printObjData.DataEntryBook = obj.DataEntryBook;
                            printObjData.DumpQty = obj.DumpQty;
                            printObjData.StreetQty = obj.StreetQty;
                            printObjData.LiquidQty = obj.LiquidQty;
                            printObjData.Note = obj.Note;
                            printObjData.UserId = obj.UserId;
                            
                            dbMain.QrPrinteds.Add(printObjData);


                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "QR Print Details Added Successfully";
                            result.messageMar = "QR प्रिंट तपशील यशस्वीरित्या समाविष्ट केले";

                        }
                    }
                    else
                    {
                        result.status = "Error";
                        result.message = "User Name  not Exist";
                        result.messageMar = "वापरकर्ता नाव अस्तित्वात नाही..";

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                result.status = "Error";
                result.message = "Something is wrong,Try Again.. ";
                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                return result;
            }


        }



        public async Task<List<QrPrintedVM>> GetAllQrPrintDetailsAsync()
        {
            List<QrPrintedVM> result = new List<QrPrintedVM>();
            try
            {
                using (dbMain)
                {
                    result = await dbMain.QrPrinteds.Select(a => new QrPrintedVM
                    {
                        PrintId = a.PrintId,
                        ULBId = a.ULBId,
                        PrintDate = a.PrintDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        UserId = a.UserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        UpdateUserId = a.UpdateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UserId).Select(e => e.Username).FirstOrDefault(),
                        UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserId).Select(e => e.Username).FirstOrDefault()

                    }).ToListAsync();
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }
        public bool IsOldUlb(DateTime? ulbDate)
        {
            DateTime dtToday = DateTime.Now;
            DateTime dt = ulbDate ?? DateTime.Now;
            
            var diffOfDate = (dtToday - dt).TotalDays;
            return diffOfDate > 15;
        }
        public async Task<QrPrintedVM> GetQrPrintDetailsAsync(int Id)
        {
            QrPrintedVM result = new QrPrintedVM();
            try
            {
                using (dbMain)
                {
                    result = await dbMain.QrPrinteds.Where(a => a.PrintId == Id).Select(a => new QrPrintedVM
                    {
                        PrintId = a.PrintId,
                        ULBId = a.ULBId,
                        PrintDate = a.PrintDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        UserId = a.UserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UserId).Select(e => e.Username).FirstOrDefault(),
                        UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserId).Select(e => e.Username).FirstOrDefault()

                    }).FirstOrDefaultAsync();

                }


                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }

        public async Task<Result> SaveQrSentAsync(QrSentVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var sentObj = await dbMain.QrSents.Where(a => a.SentId == obj.SentId).FirstOrDefaultAsync();

                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.UserId))
                    {
                        if (sentObj != null)
                        {
                            sentObj.ULBId = obj.ULBId;
                            sentObj.HouseQty = obj.HouseQty;
                            sentObj.HouseGreen = obj.HouseGreen;
                            sentObj.HouseBlue = obj.HouseBlue;
                            sentObj.BannerAcrylic = obj.BannerAcrylic;
                            sentObj.DumpAcrylic = obj.DumpAcrylic;
                            sentObj.AbhiprayForm = obj.AbhiprayForm;
                            sentObj.DumpQty = obj.DumpQty;
                            sentObj.StreetQty = obj.StreetQty;
                            sentObj.LiquidQty = obj.LiquidQty;
                            sentObj.Note = obj.Note;
                            sentObj.UserId = obj.UserId;
                            sentObj.UpdationDate = DateTime.Now;
                            sentObj.UpdateUserId = obj.UserId;
                            sentObj.DisclaimerForm = obj.DisclaimerForm;
                            sentObj.DataEntryBook = obj.DataEntryBook;
                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "QR Sent Details Updated Successfully";
                            result.messageMar = "QR सेंट तपशील यशस्वीरित्या बदलले";

                        }
                        else
                        {
                            var sentObjData = new QrSent();
                            sentObjData.ULBId = obj.ULBId;
                            sentObjData.SentDate = obj.SentDate;
                            sentObjData.CreationDate = DateTime.Now;
                            sentObjData.UpdationDate = DateTime.Now;
                            sentObjData.HouseQty = obj.HouseQty;
                            sentObjData.HouseGreen = obj.HouseGreen;
                            sentObjData.HouseBlue = obj.HouseBlue;
                            sentObjData.BannerAcrylic = obj.BannerAcrylic;
                            sentObjData.DumpAcrylic = obj.DumpAcrylic;
                            sentObjData.AbhiprayForm = obj.AbhiprayForm;
                            sentObjData.DumpQty = obj.DumpQty;
                            sentObjData.StreetQty = obj.StreetQty;
                            sentObjData.LiquidQty = obj.LiquidQty;
                            sentObjData.Note = obj.Note;
                            sentObjData.UserId = obj.UserId;
                            sentObjData.DisclaimerForm = obj.DisclaimerForm;
                            sentObjData.DataEntryBook = obj.DataEntryBook;

                            dbMain.QrSents.Add(sentObjData);


                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "QR Sent Details Added Successfully";
                            result.messageMar = "QR सेंट तपशील यशस्वीरित्या समाविष्ट केले";

                        }
                    }
                    else
                    {
                        result.status = "Error";
                        result.message = "User Name  not Exist";
                        result.messageMar = "वापरकर्ता नाव अस्तित्वात नाही..";

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                result.status = "Error";
                result.message = "Something is wrong,Try Again.. ";
                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                return result;
            }


        }


        public async Task<List<QrSentVM>> GetAllQrSentDetailsAsync()
        {
            List<QrSentVM> result = new List<QrSentVM>();
            try
            {
                using (dbMain)
                {
                    result = await dbMain.QrSents.Select(a => new QrSentVM
                    {
                        SentId = a.SentId,
                        ULBId = a.ULBId,
                        SentDate = a.SentDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        UserId = a.UserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UserId).Select(e => e.Username).FirstOrDefault(),
                        UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserId).Select(e => e.Username).FirstOrDefault()

                    }).ToListAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }

        public async Task<QrSentVM> GetQrSentDetailsAsync(int Id)
        {
            QrSentVM result = new QrSentVM();
            try
            {
                using (dbMain)
                {
                    result = await dbMain.QrSents.Where(a => a.SentId == Id).Select(a => new QrSentVM
                    {
                        SentId = a.SentId,
                        ULBId = a.ULBId,
                        SentDate = a.SentDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        UserId = a.UserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UserId).Select(e => e.Username).FirstOrDefault(),
                        UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserId).Select(e => e.Username).FirstOrDefault()

                    }).FirstOrDefaultAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }

        public async Task<List<ActiveULBVM>> GetActiveULBDetailsAsync()
        {
            List<ActiveULBVM> result = new List<ActiveULBVM>();


            try
            {
                using (dbMain)
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@dateDiff", Value = 15 }
                                                };
                    var data = await dbMain.sp_Get_ActiveULB_results.FromSqlRaw<sp_Get_ActiveULB_result>("EXEC sp_Get_ActiveULB @dateDiff", parms.ToArray()).ToListAsync();

                    if (data != null && data.Count > 0)
                    {
                        result = data.Select(a => new ActiveULBVM
                        {
                            Id = a.Id,
                            AppName = a.AppName,
                            LastUpdateDate = a.LastUpdateDate,
                            ULBId = a.ULBId,
                            dtDiff = a.dtDiff

                        }).ToList();

                    }
                    
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }


        }



        public async Task<List<ULBStatusVM>> GetULBStatusAsync(int ulbId)
        {
            List<ULBStatusVM> result = new List<ULBStatusVM>();


            try
            {
                using (dbMain)
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@ulbId", Value = ulbId }
                                                };
                    var data = await dbMain.sp_getULB_statusById_Results.FromSqlRaw<sp_getULB_statusById_Result>("EXEC sp_getULB_statusById @ulbId", parms.ToArray()).ToListAsync();

                    if (data != null && data.Count > 0)
                    {
                        result = data.Select(a => new ULBStatusVM
                        {
                            formType = a.formType,
                            sent = a.sent,
                            prin = a.prin,
                            receive = a.receive
                            
                        }).ToList();

                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }


        }

        public async Task<Result> SaveQrReceiveAsync(QrReceiveVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var receiveObj = await dbMain.QrReceives.Where(a => a.ReceiveId == obj.ReceiveId).FirstOrDefaultAsync();

                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.UserId))
                    {
                        if (receiveObj != null)
                        {
                            receiveObj.ULBId = obj.ULBId;
                            receiveObj.HouseQty = obj.HouseQty;
                            receiveObj.HouseGreen = obj.HouseGreen;
                            receiveObj.HouseBlue = obj.HouseBlue;
                            receiveObj.BannerAcrylic = obj.BannerAcrylic;
                            receiveObj.DumpAcrylic = obj.DumpAcrylic;
                            receiveObj.AbhiprayForm = obj.AbhiprayForm;
                            receiveObj.DumpQty = obj.DumpQty;
                            receiveObj.StreetQty = obj.StreetQty;
                            receiveObj.LiquidQty = obj.LiquidQty;
                            receiveObj.Note = obj.Note;
                            receiveObj.UserId = obj.UserId;
                            receiveObj.UpdationDate = DateTime.Now;
                            receiveObj.UpdateUserId = obj.UserId;
                            receiveObj.DisclaimerForm = obj.DisclaimerForm;
                            receiveObj.DataEntryBook = obj.DataEntryBook;

                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "QR Receive Details Updated Successfully";
                            result.messageMar = "QR प्राप्त तपशील यशस्वीरित्या अद्यतनित केले";

                        }
                        else
                        {
                            var receiveObjData = new QrReceive();
                            receiveObjData.ULBId = obj.ULBId;
                            receiveObjData.ReceiveDate = obj.ReceiveDate;
                            receiveObjData.CreationDate = DateTime.Now;
                            receiveObjData.UpdationDate = DateTime.Now;
                            receiveObjData.HouseQty = obj.HouseQty;
                            receiveObjData.HouseGreen = obj.HouseGreen;
                            receiveObjData.HouseBlue = obj.HouseBlue;
                            receiveObjData.BannerAcrylic = obj.BannerAcrylic;
                            receiveObjData.DumpAcrylic = obj.DumpAcrylic;
                            receiveObjData.AbhiprayForm = obj.AbhiprayForm;
                            receiveObjData.DumpQty = obj.DumpQty;
                            receiveObjData.StreetQty = obj.StreetQty;
                            receiveObjData.LiquidQty = obj.LiquidQty;
                            receiveObjData.Note = obj.Note;
                            receiveObjData.UserId = obj.UserId;
                            receiveObjData.DisclaimerForm = obj.DisclaimerForm;
                            receiveObjData.DataEntryBook = obj.DataEntryBook;
                            dbMain.QrReceives.Add(receiveObjData);


                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "QR Receive Details Added Successfully";
                            result.messageMar = "QR प्राप्त तपशील यशस्वीरित्या जोडले";

                        }
                    }
                    else
                    {
                        result.status = "Error";
                        result.message = "User Name  not Exist";
                        result.messageMar = "वापरकर्ता नाव अस्तित्वात नाही..";

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                result.status = "Error";
                result.message = "Something is wrong,Try Again.. ";
                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                return result;
            }


        }

        public async Task<List<QrReceiveVM>> GetAllQrReceiveDetailsAsync()
        {
            List<QrReceiveVM> result = new List<QrReceiveVM>();
            try
            {
                using (dbMain)
                {
                    result = await dbMain.QrReceives.Select(a => new QrReceiveVM
                    {
                        ReceiveId = a.ReceiveId,
                        ULBId = a.ULBId,
                        ReceiveDate = a.ReceiveDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        UserId = a.UserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UserId).Select(e => e.Username).FirstOrDefault(),
                        UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserId).Select(e => e.Username).FirstOrDefault()

                    }).ToListAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }

        public async Task<QrReceiveVM> GetQrReceiveDetailsAsync(int Id)
        {
            QrReceiveVM result = new QrReceiveVM();
            try
            {
                using (dbMain)
                {
                    result = await dbMain.QrReceives.Where(a => a.ReceiveId == Id).Select(a => new QrReceiveVM
                    {
                        ReceiveId = a.ReceiveId,
                        ULBId = a.ULBId,
                        ReceiveDate = a.ReceiveDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        UserId = a.UserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UserId).Select(e => e.Username).FirstOrDefault(),
                        UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.UpdateUserId).Select(e => e.Username).FirstOrDefault()

                    }).FirstOrDefaultAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }

        }




    }
}
