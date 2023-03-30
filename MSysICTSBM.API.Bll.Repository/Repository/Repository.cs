
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MSysICTSBM.API.Bll.ViewModels.Models;
using MSysICTSBM.Dal.DataContexts.Models.DB;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainContext;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainModels;
using MSysICTSBM.Dal.DataContexts.Models.DB.MainSPModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSysICTSBM.API.Bll.Repository.Repository
{
    public class Repository : IRepository
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
                        if (string.IsNullOrEmpty(userobj.imoNo))
                        {

                            userobj.imoNo = obj.imiNo;
                            await dbMain.SaveChangesAsync();
                            user.type = "";
                            user.typeId = 0;
                            user.userId = userobj.Id;
                            user.userLoginId = userobj.Username;
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = userobj.Type;

                            user.status = "success";
                            user.message = "Login Successfully";
                            user.messageMar = "लॉगिन यशस्वी";
                            user.token = await LoginAsync(userobj.Username, userobj.Type, userobj.Id);
                        }
                        else if (userobj.imoNo == obj.imiNo)
                        {
                            user.type = "";
                            user.typeId = 0;
                            user.userId = userobj.Id;
                            user.userLoginId = userobj.Username;
                            user.userPassword = "";
                            user.imiNo = "";
                            user.EmpType = userobj.Type;

                            user.status = "success";
                            user.message = "Login Successfully";
                            user.messageMar = "लॉगिन यशस्वी";
                            user.token = await LoginAsync(userobj.Username, userobj.Type, userobj.Id);
                        }
                        else
                        {
                            user.userId = 0;
                            user.userLoginId = "";
                            user.userPassword = "";
                            user.status = "error";
                            user.gtFeatures = false;
                            user.imiNo = "";
                            user.EmpType = "";
                            user.message = "User is already login with another mobile.";
                            user.messageMar = "वापरकर्ता दुसर्या मोबाइलवर आधीपासूनच लॉगिन आहे.";
                        }

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
                    expires: DateTime.Now.AddYears(1),
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

                            ulbObj.AhwalCount = obj.AhwalCount;
                            ulbObj.TrainingCount = obj.TrainingCount;

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
                            ulbObjData.UpdateDate = DateTime.Now;  // In 15 days ULB working or not that's why Update date insert on save time.

                            ulbObjData.AhwalCount = obj.AhwalCount;
                            ulbObjData.TrainingCount = obj.TrainingCount;

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
                // result.message = "Something is wrong,Try Again.. ";
                result.message = ex.Message;
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
                        if (await dbMain.EmployeeMasters.AnyAsync(a => a.Username == obj.Username))
                        {


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
            catch (Exception ex)
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
                    var printObj = await dbMain.QrPrinteds.Where(a => a.PrintId == obj.Id).FirstOrDefaultAsync();

                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.UserId))
                    {
                        if (printObj != null)
                        {
                            printObj.ULBId = obj.ULBId;
                            printObj.HouseQty = obj.HouseQty;
                            printObj.HouseGreen = obj.HouseGreen;
                            printObj.HouseBlue = obj.HouseBlue;
                            printObj.DumpQty = obj.DumpQty;
                            printObj.StreetQty = obj.StreetQty;
                            printObj.LiquidQty = obj.LiquidQty;

                            printObj.BannerAcrylic = obj.BannerAcrylic;
                            printObj.DumpAcrylic = obj.DumpAcrylic;

                            printObj.AbhiprayForm = obj.AbhiprayForm;

                            printObj.DisclaimerForm = obj.DisclaimerForm;

                            printObj.DataEntryBook = obj.DataEntryBook;


                            printObj.Note = obj.Note;
                            //printObj.UserId = obj.UserId;
                            printObj.UpdationDate = DateTime.Now;
                            printObj.UpdateUserId = obj.UserId;

                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            // result.message = "QR Print Details Updated Successfully";
                            // result.messageMar = "QR प्रिंट तपशील यशस्वीरित्या बदलले";
                            result.message = "Data Updated Successfully";
                            result.messageMar = "डेटा यशस्वीरित्या अपडेट केला";


                        }
                        else
                        {
                            var printObjData = new QrPrinted();
                            printObjData.ULBId = obj.ULBId;
                            printObjData.PrintDate = obj.Date;
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
                            printObjData.CreateUserId = obj.UserId;
                            printObjData.QrMId = obj.QrMId;

                            dbMain.QrPrinteds.Add(printObjData);


                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            //result.message = "QR Print Details Added Successfully";
                            //result.messageMar = "QR प्रिंट तपशील यशस्वीरित्या समाविष्ट केले";
                            result.message = "Data Added Successfully";
                            result.messageMar = "डेटा यशस्वीरित्या जोडला गेला";


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
                //result.message = "Something is wrong,Try Again.. ";
                result.message = ex.Message;
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
                        Id = a.PrintId,
                        ULBId = a.ULBId,
                        Date = a.PrintDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        CreateUserId = a.CreateUserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        UpdateUserId = a.UpdateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.CreateUserId).Select(e => e.Username).FirstOrDefault(),
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
                        Id = a.PrintId,
                        ULBId = a.ULBId,
                        Date = a.PrintDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        CreateUserId = a.CreateUserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.CreateUserId).Select(e => e.Username).FirstOrDefault(),
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
                    var sentObj = await dbMain.QrSents.Where(a => a.SentId == obj.Id).FirstOrDefaultAsync();

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
                            //sentObj.UserId = obj.UserId;
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
                            sentObjData.SentDate = obj.Date;
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
                            sentObjData.CreateUserId = obj.UserId;
                            sentObjData.DisclaimerForm = obj.DisclaimerForm;
                            sentObjData.DataEntryBook = obj.DataEntryBook;
                            sentObjData.QrMId = obj.QrMId;

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
                //result.message = "Something is wrong,Try Again.. ";
                result.message = ex.Message;
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
                        Id = a.SentId,
                        ULBId = a.ULBId,
                        Date = a.SentDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        CreateUserId = a.CreateUserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.CreateUserId).Select(e => e.Username).FirstOrDefault(),
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
                        Id = a.SentId,
                        ULBId = a.ULBId,
                        Date = a.SentDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        CreateUserId = a.CreateUserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.CreateUserId).Select(e => e.Username).FirstOrDefault(),
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
                            print = a.prin,
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


        public async Task<List<ULBDocStatusVMDocData>> GetULBDocStatusAsync(int ulbId, int docId)
        {
            List<ULBDocStatusVMDocData> result = new List<ULBDocStatusVMDocData>();
            List<DataAll> da = new List<DataAll>();
            List<Send> ss = new List<Send>();
            List<DigCopy> dc = new List<DigCopy>();
            List<HardCopy> hc = new List<HardCopy>();

            try
            {
                using (dbMain)
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@ulbId", Value = ulbId },
                                                    new SqlParameter { ParameterName = "@DocId", Value = docId }
                                                };
                    var data = await dbMain.sp_getULB_DocStatus_Results.FromSqlRaw<sp_getULB_DocStatus_Result>("EXEC sp_getULB_DocStatus @ulbId,@DocId", parms.ToArray()).ToListAsync();
                    var docData = await dbMain.DocMasters.ToListAsync();
                    var docSubData = await dbMain.DocSubMasters.Where(s => s.DocId == docId && s.IsActive == true).ToListAsync();


                    var count = await dbMain.ULB_Details.Where(m => m.Id == ulbId).FirstOrDefaultAsync();
                    if (docId == 3)
                    {
                        if (count.AhwalCount == null || count.AhwalCount == 0)
                        {
                            count.AhwalCount = 1;
                        }
                        docSubData = await dbMain.DocSubMasters.Where(s => s.DocId == docId && s.IsActive == true).Take(Convert.ToInt32(count.AhwalCount)).ToListAsync();
                    }

                    if (docId == 4)
                    {
                        if (count.TrainingCount == null || count.TrainingCount == 0)
                        {
                            count.TrainingCount = 6;
                        }
                        docSubData = await dbMain.DocSubMasters.Where(s => s.DocId == docId && s.IsActive == true).Take(Convert.ToInt32(count.TrainingCount)).ToListAsync();
                    }


                    if (docData != null && docSubData != null)
                    {
                        foreach (var a in docSubData)
                        {
                            var data2 = data.Where(c => c.DocSentStatus == true && c.DocSubName == a.DocSubName).ToList();
                            foreach (var c in data2)
                            {
                                ss.Add(new Send()
                                {
                                    Id = c.Id,
                                    Note = c.DocSentNote,
                                    UserName = c.DocSentCreateUserName,
                                    DocSubName = a.DocSubName,
                                    CreationDate = c.DocSentCreateDate,

                                });
                            }


                            var data3 = data.Where(c => c.DocDigCopyRecStatus == true && c.DocSubName == a.DocSubName).ToList();
                            foreach (var d in data3)
                            {
                                dc.Add(new DigCopy()
                                {
                                    Id = d.Id,
                                    Note = d.DocSentNote,
                                    UserName = d.DocSentCreateUserName,
                                    DocSubName = a.DocSubName,
                                    CreationDate = d.DocSentCreateDate,

                                });
                            }

                            var data4 = data.Where(c => c.DocHardCopyRecStatus == true && c.DocSubName == a.DocSubName).ToList();
                            foreach (var e in data4)
                            {
                                hc.Add(new HardCopy()
                                {
                                    Id = e.Id,
                                    Note = e.DocSentNote,
                                    UserName = e.DocSentCreateUserName,
                                    DocSubName = a.DocSubName,
                                    CreationDate = e.DocSentCreateDate,
                                });
                            }


                            //var data1 = data.Where(c => c.DocSubName == a.DocSubName).ToList();
                            //foreach (var b in data1)
                            //{
                            da.Add(new DataAll()
                            {
                                DocSubNameNew = a.DocSubName,

                                SendData = ss.Where(s => s.DocSubName == a.DocSubName).ToList(),
                                DCData = dc.Where(p => p.DocSubName == a.DocSubName).ToList(),
                                HCData = hc.Where(p => p.DocSubName == a.DocSubName).ToList(),

                                DocSentStatus = data2.Count > 0 ? true : false,
                                DocDigCopyRecStatus = data3.Count > 0 ? true : false,
                                DocHardCopyRecStatus = data4.Count > 0 ? true : false,
                            });
                            //}

                            result.Add(new ULBDocStatusVMDocData()
                            {
                                DocName = dbMain.DocMasters.Where(s => s.Id == a.DocId).Select(s => s.DocName).FirstOrDefault(),
                                DocSubName = a.DocSubName,
                                DocSubId = dbMain.DocSubMasters.Where(c => c.DocSubName == a.DocSubName).Select(s => s.Id).FirstOrDefault(),
                                DataAll = da.Where(s => s.DocSubNameNew == a.DocSubName).ToList(),

                            });
                        }
                    }


                    //// Milind
                    //if (data != null && data.Count > 0)
                    //{
                    //    result = data.Select(a => new ULBDocStatusVM
                    //    {
                    //        AppName = a.AppName,
                    //        DocName = a.DocName,
                    //        DocSubName = a.DocSubName,
                    //        DocSentStatus = a.DocSentStatus,
                    //        DocSentCreateUserName = a.DocSentCreateUserName,
                    //        DocSentUpdateUserName = a.DocSentUpdateUserName,
                    //        DocSentCreateDate = a.DocSentCreateDate,
                    //        DocSentUpdateDate = a.DocSentUpdateDate,
                    //        DocSentNote = a.DocSentNote,

                    //        DocDigCopyRecStatus = a.DocDigCopyRecStatus,
                    //        DocDigCopyRecCreateUserName = a.DocDigCopyRecCreateUserName,
                    //        DocDigCopyRecUpdateUserName = a.DocDigCopyRecUpdateUserName,
                    //        DocDigCopyRecCreateDate = a.DocDigCopyRecCreateDate,
                    //        DocDigCopyRecUpdateDate = a.DocDigCopyRecUpdateDate,
                    //        DocDigCopyNote = a.DocDigCopyNote,

                    //        DocHardCopyRecStatus = a.DocHardCopyRecStatus,
                    //        DocHardCopyRecCreateUserName = a.DocHardCopyRecCreateUserName,
                    //        DocHardCopyRecUpdateUserName = a.DocHardCopyRecUpdateUserName,
                    //        DocHardCopyRecCreateDate = a.DocHardCopyRecCreateDate,
                    //        DocHardCopyRecUpdateDate = a.DocHardCopyRecUpdateDate,
                    //        DocHardCopyNote = a.DocHardCopyNote

                    //    }).ToList();

                    //}

                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }


        }

        public async Task<List<ULBDocStatusVMNew>> GetAllULBDocStatusAsync(int ulbId)
        {
            List<ULBDocStatusVMNew> result = new List<ULBDocStatusVMNew>();
            List<SubDatum> sd = new List<SubDatum>();

            try
            {
                using (dbMain)
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@ulbId", Value = ulbId }
                                                };
                    var data = await dbMain.sp_getULB_AllDocStatus_Results.FromSqlRaw<sp_getULB_DocStatus_Result>("EXEC sp_getULB_All_DocStatus @ulbId", parms.ToArray()).ToListAsync();
                    var docData = await dbMain.DocMasters.Where(s => s.IsActive == true).ToListAsync();

                    if (docData != null)
                    {
                        foreach (var a in docData)
                        {
                            var data1 = data.Where(c => c.DocName == a.DocName).ToList();

                            // a.Id=3  that Condition For Awhal 
                            var count = await dbMain.ULB_Details.Where(m => m.Id == ulbId).FirstOrDefaultAsync();
                            if (a.Id == 3 && count != null)
                            {
                                if (count.AhwalCount == null || count.AhwalCount == 0)
                                {
                                    count.AhwalCount = 1;
                                }
                                data1 = data.Where(c => c.DocName == a.DocName).Take(Convert.ToInt32(count.AhwalCount)).ToList();

                            }
                            // a.Id=4  that Condition For Training 
                            if (a.Id == 4 && count != null)
                            {
                                if (count.TrainingCount == null || count.TrainingCount == 0)
                                {
                                    count.TrainingCount = 6;
                                }
                                data1 = data.Where(c => c.DocName == a.DocName).Take(Convert.ToInt32(count.TrainingCount)).ToList();
                            }

                            foreach (var b in data1)
                            {
                                sd.Add(new SubDatum()
                                {
                                    DocNameNew = a.DocName,
                                    DocSubName = b.DocSubName,
                                    DocSentStatus = Convert.ToBoolean(b.DocSentStatus),
                                    DocDigCopyRecStatus = Convert.ToBoolean(b.DocDigCopyRecStatus),
                                    DocHardCopyRecStatus = Convert.ToBoolean(b.DocHardCopyRecStatus),

                                });
                            }

                            result.Add(new ULBDocStatusVMNew()
                            {
                                AppName = dbMain.ULB_Details.Where(s => s.Id == ulbId).Select(s => s.AppName).FirstOrDefault(),
                                Id = a.Id,
                                DocName = a.DocName,
                                SubData = sd.Where(s => s.DocNameNew == a.DocName).ToList()
                            });
                        }
                    }


                    // Milind Code

                    //if (data != null && data.Count > 0 )
                    //{
                    //    result = data.Select(a => new ULBDocStatusVMNew
                    //    {
                    //        AppName = a.AppName,
                    //        DocName = a.DocName,

                    //        //DocSubName = a.DocSubName,
                    //        //DocSentStatus = a.DocSentStatus,
                    //        //DocSentCreateUserName = a.DocSentCreateUserName,
                    //        //DocSentUpdateUserName = a.DocSentUpdateUserName,
                    //        //DocSentCreateDate = a.DocSentCreateDate,
                    //        //DocSentUpdateDate = a.DocSentUpdateDate,
                    //        //DocDigCopyRecStatus = a.DocDigCopyRecStatus,
                    //        //DocDigCopyRecCreateUserName = a.DocDigCopyRecCreateUserName,
                    //        //DocDigCopyRecUpdateUserName = a.DocDigCopyRecUpdateUserName,
                    //        //DocDigCopyRecCreateDate = a.DocDigCopyRecCreateDate,
                    //        //DocDigCopyRecUpdateDate = a.DocDigCopyRecUpdateDate,
                    //        //DocHardCopyRecStatus = a.DocHardCopyRecStatus,
                    //        //DocHardCopyRecCreateUserName = a.DocHardCopyRecCreateUserName,
                    //        //DocHardCopyRecUpdateUserName = a.DocHardCopyRecUpdateUserName,
                    //        //DocHardCopyRecCreateDate = a.DocHardCopyRecCreateDate,
                    //        //DocHardCopyRecUpdateDate = a.DocHardCopyRecUpdateDate

                    //    }).ToList();



                    //}

                    // Milind Code End

                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                return result;

            }


        }


        public async Task<List<ULBFormStatusVMNew>> GetULBFormStatusNewAsync(int ulbId)
        {
            List<ULBFormStatusVMNew> result = new List<ULBFormStatusVMNew>();
            List<FormDataAll> da = new List<FormDataAll>();

            List<Print> print = new List<Print>();
            List<Sent> sent = new List<Sent>();
            List<Receive> receive = new List<Receive>();

            try
            {
                using (dbMain)
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                                                {
                                                    // Create parameter(s)    
                                                    new SqlParameter { ParameterName = "@ulbId", Value = ulbId },
                                                };
                    var data = await dbMain.sp_getULB_FormStatus_Result.FromSqlRaw<sp_getULB_FormStatus_Result>("EXEC sp_getULB_FormStatus @ulbId", parms.ToArray()).ToListAsync();
                    var FormData = await dbMain.QrFormMasters.Where(c => c.Status == true).ToListAsync();

                    if (FormData != null)
                    {
                        foreach (var a in FormData)
                        {
                            var dataPrint = data.Where(c => c.PrintStatus == true && c.FormName == a.FormName).OrderByDescending(a => a.Id).ToList();
                            foreach (var c in dataPrint)
                            {
                                print.Add(new Print()
                                {
                                    Id = c.Id,
                                    HouseQty = c.HouseQty,
                                    HouseGreen = c.HouseGreen,
                                    HouseBlue = c.HouseBlue,
                                    BannerAcrylic = c.BannerAcrylic,
                                    DumpAcrylic = c.DumpAcrylic,
                                    AbhiprayForm = c.AbhiprayForm,
                                    DisclaimerForm = c.DisclaimerForm,
                                    DataEntryBook = c.DataEntryBook,
                                    DumpQty = c.DumpQty,
                                    StreetQty = c.StreetQty,
                                    LiquidQty = c.LiquidQty,

                                    UserName = c.Name,
                                    FormName = a.FormName,
                                    CreationDate = c.CreationDate,

                                });
                            }


                            var dataSend = data.Where(c => c.SendStatus == true && c.FormName == a.FormName).OrderByDescending(a => a.Id).ToList();
                            foreach (var d in dataSend)
                            {
                                sent.Add(new Sent()
                                {
                                    Id = d.Id,
                                    HouseQty = d.HouseQty,
                                    HouseGreen = d.HouseGreen,
                                    HouseBlue = d.HouseBlue,
                                    BannerAcrylic = d.BannerAcrylic,
                                    DumpAcrylic = d.DumpAcrylic,
                                    AbhiprayForm = d.AbhiprayForm,
                                    DisclaimerForm = d.DisclaimerForm,
                                    DataEntryBook = d.DataEntryBook,
                                    DumpQty = d.DumpQty,
                                    StreetQty = d.StreetQty,
                                    LiquidQty = d.LiquidQty,

                                    UserName = d.Name,
                                    FormName = a.FormName,
                                    CreationDate = d.CreationDate,
                                });
                            }

                            var dataReceive = data.Where(c => c.ReceiveStatus == true && c.FormName == a.FormName).OrderByDescending(a => a.Id).ToList();
                            foreach (var e in dataReceive)
                            {
                                receive.Add(new Receive()
                                {
                                    Id = e.Id,
                                    HouseQty = e.HouseQty,
                                    HouseGreen = e.HouseGreen,
                                    HouseBlue = e.HouseBlue,
                                    BannerAcrylic = e.BannerAcrylic,
                                    DumpAcrylic = e.DumpAcrylic,
                                    AbhiprayForm = e.AbhiprayForm,
                                    DisclaimerForm = e.DisclaimerForm,
                                    DataEntryBook = e.DataEntryBook,
                                    DumpQty = e.DumpQty,
                                    StreetQty = e.StreetQty,
                                    LiquidQty = e.LiquidQty,

                                    UserName = e.Name,
                                    FormName = a.FormName,
                                    CreationDate = e.CreationDate,
                                });
                            }



                            da.Add(new FormDataAll()
                            {
                                FormNameNew = a.FormName,

                                PrintData = print.Where(s => s.FormName == a.FormName).ToList(),
                                SendData = sent.Where(p => p.FormName == a.FormName).ToList(),
                                ReceiveData = receive.Where(p => p.FormName == a.FormName).ToList(),

                                PrintStatus = dataPrint.Count > 0 ? true : false,
                                SendStatus = dataSend.Count > 0 ? true : false,
                                ReceiveStatus = dataReceive.Count > 0 ? true : false,
                            });


                            result.Add(new ULBFormStatusVMNew()
                            {
                                AppName = dbMain.ULB_Details.Where(s => s.Id == ulbId).Select(s => s.AppName).FirstOrDefault(),
                                FormName = a.FormName,
                                QrMId = a.Id,
                                DataAll = da.Where(s => s.FormNameNew == a.FormName).ToList(),

                            });
                        }
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


        public async Task<ULBFormStatusVM> GetULBFormStatusAsync(int ulbId)
        {
            ULBFormStatusVM result = new ULBFormStatusVM();
            try
            {
                using (dbMain)
                {
                    result.QrCode.Printed = await dbMain.QrPrinteds.Where(a => a.ULBId == ulbId && (a.HouseQty > 0 || a.HouseBlue > 0 || a.HouseGreen > 0 || a.DumpQty > 0 || a.StreetQty > 0)).OrderByDescending(c => c.PrintId).Select(a => new QrPrintedVM
                    {
                        HouseQty = a.HouseQty ?? 0,
                        HouseBlue = a.HouseBlue ?? 0,
                        HouseGreen = a.HouseGreen ?? 0,
                        DumpQty = a.DumpQty ?? 0,
                        StreetQty = a.StreetQty ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),


                    }).ToListAsync();
                    result.QrCode.isPrinted = result.QrCode.Printed != null && result.QrCode.Printed.Count > 0 ? true : false;
                    result.QrCode.Sent = await dbMain.QrSents.Where(a => a.ULBId == ulbId && (a.HouseQty > 0 || a.HouseBlue > 0 || a.HouseGreen > 0 || a.DumpQty > 0 || a.StreetQty > 0)).OrderByDescending(c => c.SentId).Select(a => new QrSentVM
                    {
                        HouseQty = a.HouseQty ?? 0,
                        HouseBlue = a.HouseBlue ?? 0,
                        HouseGreen = a.HouseGreen ?? 0,
                        DumpQty = a.DumpQty ?? 0,
                        StreetQty = a.StreetQty ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),

                    }).ToListAsync();
                    result.QrCode.isSent = result.QrCode.Sent != null && result.QrCode.Sent.Count > 0 ? true : false;

                    result.QrCode.Received = await dbMain.QrReceives.Where(a => a.ULBId == ulbId && (a.HouseQty > 0 || a.HouseBlue > 0 || a.HouseGreen > 0 || a.DumpQty > 0 || a.StreetQty > 0)).OrderByDescending(c => c.ReceiveId).Select(a => new QrReceiveVM
                    {
                        HouseQty = a.HouseQty ?? 0,
                        HouseBlue = a.HouseBlue ?? 0,
                        HouseGreen = a.HouseGreen ?? 0,
                        DumpQty = a.DumpQty ?? 0,
                        StreetQty = a.StreetQty ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.QrCode.isReceived = result.QrCode.Received != null && result.QrCode.Received.Count > 0 ? true : false;


                    result.Banners.Printed = await dbMain.QrPrinteds.Where(a => a.ULBId == ulbId && (a.BannerAcrylic > 0 || a.DumpAcrylic > 0)).OrderByDescending(c => c.PrintId).Select(a => new QrPrintedVM
                    {
                        BannerAcrylic = a.BannerAcrylic ?? 0,
                        DumpAcrylic = a.DumpAcrylic ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Banners.isPrinted = result.Banners.Printed != null && result.Banners.Printed.Count > 0 ? true : false;

                    result.Banners.Sent = await dbMain.QrSents.Where(a => a.ULBId == ulbId && (a.BannerAcrylic > 0 || a.DumpAcrylic > 0)).OrderByDescending(c => c.SentId).Select(a => new QrSentVM
                    {
                        BannerAcrylic = a.BannerAcrylic ?? 0,
                        DumpAcrylic = a.DumpAcrylic ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Banners.isSent = result.Banners.Sent != null && result.Banners.Sent.Count > 0 ? true : false;

                    result.Banners.Received = await dbMain.QrReceives.Where(a => a.ULBId == ulbId && (a.BannerAcrylic > 0 || a.DumpAcrylic > 0)).OrderByDescending(c => c.ReceiveId).Select(a => new QrReceiveVM
                    {
                        BannerAcrylic = a.BannerAcrylic ?? 0,
                        DumpAcrylic = a.DumpAcrylic ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Banners.isReceived = result.Banners.Received != null && result.Banners.Received.Count > 0 ? true : false;


                    result.Abhipray.Printed = await dbMain.QrPrinteds.Where(a => a.ULBId == ulbId && (a.AbhiprayForm > 0)).OrderByDescending(c => c.PrintId).Select(a => new QrPrintedVM
                    {
                        AbhiprayForm = a.AbhiprayForm ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Abhipray.isPrinted = result.Abhipray.Printed != null && result.Abhipray.Printed.Count > 0 ? true : false;

                    result.Abhipray.Sent = await dbMain.QrSents.Where(a => a.ULBId == ulbId && (a.AbhiprayForm > 0)).OrderByDescending(c => c.SentId).Select(a => new QrSentVM
                    {
                        AbhiprayForm = a.AbhiprayForm ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Abhipray.isSent = result.Abhipray.Sent != null && result.Abhipray.Sent.Count > 0 ? true : false;

                    result.Abhipray.Received = await dbMain.QrReceives.Where(a => a.ULBId == ulbId && (a.AbhiprayForm > 0)).OrderByDescending(c => c.ReceiveId).Select(a => new QrReceiveVM
                    {
                        AbhiprayForm = a.AbhiprayForm ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Abhipray.isReceived = result.Abhipray.Received != null && result.Abhipray.Received.Count > 0 ? true : false;


                    result.Disclaimer.Printed = await dbMain.QrPrinteds.Where(a => a.ULBId == ulbId && (a.DisclaimerForm > 0)).OrderByDescending(c => c.PrintId).Select(a => new QrPrintedVM
                    {
                        DisclaimerForm = a.DisclaimerForm ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Disclaimer.isPrinted = result.Disclaimer.Printed != null && result.Disclaimer.Printed.Count > 0 ? true : false;

                    result.Disclaimer.Sent = await dbMain.QrSents.Where(a => a.ULBId == ulbId && (a.DisclaimerForm > 0)).OrderByDescending(c => c.SentId).Select(a => new QrSentVM
                    {
                        DisclaimerForm = a.DisclaimerForm ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Disclaimer.isSent = result.Disclaimer.Sent != null && result.Disclaimer.Sent.Count > 0 ? true : false;

                    result.Disclaimer.Received = await dbMain.QrReceives.Where(a => a.ULBId == ulbId && (a.DisclaimerForm > 0)).OrderByDescending(c => c.ReceiveId).Select(a => new QrReceiveVM
                    {
                        DisclaimerForm = a.DisclaimerForm ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),
                    }).ToListAsync();
                    result.Disclaimer.isReceived = result.Disclaimer.Received != null && result.Disclaimer.Received.Count > 0 ? true : false;


                    result.EntryBook.Printed = await dbMain.QrPrinteds.Where(a => a.ULBId == ulbId && (a.DataEntryBook > 0)).OrderByDescending(c => c.PrintId).Select(a => new QrPrintedVM
                    {
                        DataEntryBook = a.DataEntryBook ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),

                    }).ToListAsync();
                    result.EntryBook.isPrinted = result.EntryBook.Printed != null && result.EntryBook.Printed.Count > 0 ? true : false;

                    result.EntryBook.Sent = await dbMain.QrSents.Where(a => a.ULBId == ulbId && (a.DataEntryBook > 0)).OrderByDescending(c => c.SentId).Select(a => new QrSentVM
                    {
                        DataEntryBook = a.DataEntryBook ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),

                    }).ToListAsync();
                    result.EntryBook.isSent = result.EntryBook.Sent != null && result.EntryBook.Sent.Count > 0 ? true : false;


                    result.EntryBook.Received = await dbMain.QrReceives.Where(a => a.ULBId == ulbId && (a.DataEntryBook > 0)).OrderByDescending(c => c.ReceiveId).Select(a => new QrReceiveVM
                    {
                        DataEntryBook = a.DataEntryBook ?? 0,
                        CreateUserId = a.CreateUserId,
                        CreateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.CreateUserId).Select(s => s.Name).FirstOrDefault(),
                        UpdationDate = a.UpdationDate,
                        CreationDate = a.CreationDate,
                        Note = a.Note.Trim(),
                        // UpdateUserId=a.UpdateUserId,
                        // UpdateUserName = dbMain.EmployeeMasters.Where(s => s.Id == a.UpdateUserId).Select(s => s.Name).FirstOrDefault(),

                    }).ToListAsync();
                    result.EntryBook.isReceived = result.EntryBook.Received != null && result.EntryBook.Received.Count > 0 ? true : false;

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
                    var receiveObj = await dbMain.QrReceives.Where(a => a.ReceiveId == obj.Id).FirstOrDefaultAsync();

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
                            //receiveObj.UserId = obj.UserId;
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
                            receiveObjData.ReceiveDate = obj.Date;
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
                            receiveObjData.CreateUserId = obj.UserId;
                            receiveObjData.DisclaimerForm = obj.DisclaimerForm;
                            receiveObjData.DataEntryBook = obj.DataEntryBook;
                            receiveObjData.QrMId = obj.QrMId;
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
                //result.message = "Something is wrong,Try Again.. ";
                result.message = ex.Message;
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
                        Id = a.ReceiveId,
                        ULBId = a.ULBId,
                        Date = a.ReceiveDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        CreateUserId = a.CreateUserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == a.CreateUserId).Select(e => e.Username).FirstOrDefault(),
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
                        Id = a.ReceiveId,
                        ULBId = a.ULBId,
                        Date = a.ReceiveDate,
                        CreationDate = a.CreationDate,
                        HouseQty = a.HouseQty,
                        HouseGreen = a.HouseGreen,
                        HouseBlue = a.HouseBlue,
                        DumpQty = a.DumpQty,
                        StreetQty = a.StreetQty,
                        LiquidQty = a.LiquidQty,
                        Note = a.Note,
                        UpdationDate = a.UpdationDate,
                        CreateUserId = a.CreateUserId,
                        DumpAcrylic = a.DumpAcrylic,
                        AbhiprayForm = a.AbhiprayForm,
                        BannerAcrylic = a.BannerAcrylic,
                        UpdateUserId = a.UpdateUserId,
                        DisclaimerForm = a.DisclaimerForm,
                        DataEntryBook = a.DataEntryBook,
                        CreateUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.CreateUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
                        UpdateUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.UpdateUserId ?? 0)).Select(e => e.Username).FirstOrDefault()

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


        public async Task<Result> SaveULBAppDetailsAsync(ULB_App_StatusVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var ulbObj = await dbMain.ULB_App_Statuses.Where(a => a.Id == obj.Id && a.ULBId == obj.ULBId).FirstOrDefaultAsync();
                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.UserId))
                    {
                        if (ulbObj != null)
                        {
                            ulbObj.ULBId = obj.ULBId;
                            //ulbObj.CMSStatus = obj.CMSStatus;
                            //ulbObj.AppStatus = obj.AppStatus;

                            if (((ulbObj.CMSStatus is null || ulbObj.CMSStatus == false) && obj.CMSStatus == true) || (ulbObj.CMSStatus == true && obj.CMSStatus == false))
                            {
                                ulbObj.CMSDate = DateTime.Now;
                                ulbObj.CMSStatus = obj.CMSStatus;
                                ulbObj.CMSUserId = obj.UserId;
                            }
                            if (((ulbObj.AppStatus is null || ulbObj.AppStatus == false) && obj.AppStatus == true) || (ulbObj.AppStatus == true && obj.AppStatus == false))
                            {
                                ulbObj.AppDate = DateTime.Now;
                                ulbObj.AppStatus = obj.AppStatus;
                                ulbObj.AppUserId = obj.UserId;
                            }

                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "ULB App Details Updated Successfully";
                            result.messageMar = "ULB अॅप तपशील यशस्वीरित्या बदलले";

                        }
                        else
                        {
                            if (!await dbMain.ULB_App_Statuses.AnyAsync(a => a.ULBId == obj.ULBId))
                            {

                                var ulbObjData = new ULB_App_Status();

                                ulbObjData.ULBId = obj.ULBId;
                                ulbObjData.AppStatus = obj.AppStatus;
                                ulbObjData.CMSStatus = obj.CMSStatus;

                                if (obj.CMSStatus == true)
                                {
                                    ulbObjData.CMSStatus = true;
                                    ulbObjData.CMSDate = DateTime.Now;
                                    ulbObjData.CMSUserId = obj.UserId;
                                }
                                else
                                {
                                    ulbObjData.CMSStatus = false;
                                }

                                if (obj.AppStatus == true)
                                {
                                    ulbObjData.AppStatus = true;
                                    ulbObjData.AppDate = DateTime.Now;
                                    ulbObjData.AppUserId = obj.UserId;
                                }

                                else
                                {
                                    ulbObjData.AppStatus = false;
                                }


                                dbMain.ULB_App_Statuses.Add(ulbObjData);
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB App Details Added Successfully";
                                result.messageMar = "ULB अॅप तपशील यशस्वीरित्या समाविष्ट केले";
                            }
                            else
                            {

                                result.status = "Error";
                                result.message = "ULB App Details already Exist";
                                result.messageMar = "ULB अॅप तपशील आधीपासून अस्तित्वात आहेत";
                            }


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

        public async Task<ULB_App_StatusVM> GetULBAppDetailsAsync(int ulbId)
        {
            ULB_App_StatusVM result = new ULB_App_StatusVM();
            ULB_App_StatusVM result1 = new ULB_App_StatusVM();

            try
            {
                using (dbMain)
                {
                    result = await dbMain.ULB_App_Statuses.Where(a => a.ULBId == ulbId).Select(a => new ULB_App_StatusVM
                    {
                        Id = a.Id,
                        ULBId = a.ULBId,
                        CMSStatus = a.CMSStatus,
                        AppStatus = a.AppStatus,
                        CMSUserId = a.CMSUserId,
                        AppUserId = a.AppUserId,
                        CMSDate = a.CMSDate,
                        AppDate = a.AppDate,
                        CMSUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.CMSUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
                        AppUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.AppUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
                    }).FirstOrDefaultAsync();

                    if (result == null && ulbId != null)
                    {
                        result1.Id = 0;
                        result1.ULBId = ulbId;
                        result1.CMSStatus = false;
                        result1.AppStatus = false;
                        result1.CMSUserId = 0;
                        result1.AppUserId = 0;
                        result1.CMSDate = null;
                        result1.AppDate = null;
                        result1.CMSUserName = null;
                        result1.AppUserName = null;
                        result = result1;
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

        public async Task<Result> SaveULBDocMasterAsync(DocMasterVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var docObj = await dbMain.DocMasters.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
                    if (docObj != null)
                    {
                        if (!await dbMain.DocMasters.AnyAsync(a => a.DocName.ToUpper() == obj.DocName.ToUpper() && a.Id != obj.Id))
                        {
                            docObj.DocName = obj.DocName;
                            docObj.DocDate = DateTime.Now;
                            docObj.IsActive = obj.IsActive;
                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "Document Updated Successfully";
                            result.messageMar = "ULB डॉक मास्टर तपशील यशस्वीरित्या बदलले";
                        }
                        else
                        {
                            result.status = "Error";
                            result.message = "Document Name Already Exist";
                            result.messageMar = "ULB डॉक नाव आधीपासून अस्तित्वात आहे";

                        }

                    }
                    else
                    {
                        if (!await dbMain.DocMasters.AnyAsync(a => a.DocName.ToUpper() == obj.DocName.ToUpper()))
                        {
                            var docObjData = new DocMaster();
                            docObjData.DocName = obj.DocName;
                            docObjData.DocDate = DateTime.Now;

                            docObjData.IsActive = obj.IsActive;

                            dbMain.DocMasters.Add(docObjData);
                            await dbMain.SaveChangesAsync();

                            result.status = "success";
                            result.message = "Document Added Successfully";
                            result.messageMar = "ULB डॉक मास्टर तपशील यशस्वीरित्या समाविष्ट केले";
                        }
                        else
                        {
                            result.status = "Error";
                            result.message = "Document Name Already Exist";
                            result.messageMar = "ULB डॉक नाव आधीपासून अस्तित्वात आहे";

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

        public async Task<DocMasterVM> GetULBDocMasterAsync(int docId)
        {
            var result = new DocMasterVM();

            try
            {
                using (dbMain)
                {
                    result = await dbMain.DocMasters.Where(a => a.Id == docId).Select(a => new DocMasterVM
                    {
                        Id = a.Id,
                        DocName = a.DocName,
                        DocDate = a.DocDate,
                        IsActive = a.IsActive

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

        public async Task<List<DocMasterVM>> GetAllULBDocMasterAsync()
        {
            var result = new List<DocMasterVM>();

            try
            {
                using (dbMain)
                {
                    result = await dbMain.DocMasters.Select(a => new DocMasterVM
                    {
                        Id = a.Id,
                        DocName = a.DocName,
                        DocDate = a.DocDate,
                        IsActive = a.IsActive

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



        public async Task<Result> SaveULBDocSubMasterAsync(DocSubMasterVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var docObj = await dbMain.DocSubMasters.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
                    if (docObj != null)
                    {
                        if (!await dbMain.DocSubMasters.AnyAsync(a => a.DocSubName.ToUpper() == obj.DocSubName.ToUpper() && a.Id != obj.Id))
                        {
                            if (await dbMain.DocMasters.AnyAsync(a => a.Id == obj.DocId))
                            {
                                docObj.DocId = obj.DocId;
                                docObj.DocSubName = obj.DocSubName;
                                docObj.DocSubDate = DateTime.Now;

                                docObj.IsActive = obj.IsActive;
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB Doc Sub Master Details Updated Successfully";
                                result.messageMar = "ULB डॉक सब मास्टर तपशील यशस्वीरित्या बदलले";
                            }
                            else
                            {
                                result.status = "Error";
                                result.message = "ULB Doc Name Does Not Exist";
                                result.messageMar = "ULB डॉक नाव अस्तित्वात नाही";

                            }


                        }
                        else
                        {
                            result.status = "Error";
                            result.message = "ULB Doc Name already Exist";
                            result.messageMar = "ULB डॉक नाव आधीपासून अस्तित्वात आहे";

                        }

                    }
                    else
                    {
                        if (!await dbMain.DocSubMasters.AnyAsync(a => a.DocSubName.ToUpper() == obj.DocSubName.ToUpper()))
                        {

                            if (await dbMain.DocMasters.AnyAsync(a => a.Id == obj.DocId))
                            {
                                var docObjData = new DocSubMaster();
                                docObjData.DocId = obj.DocId;
                                docObjData.DocSubName = obj.DocSubName;
                                docObjData.DocSubDate = DateTime.Now;

                                docObjData.IsActive = obj.IsActive;
                                dbMain.DocSubMasters.Add(docObjData);
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB Doc Sub Master Details Added Successfully";
                                result.messageMar = "ULB डॉक सब मास्टर तपशील यशस्वीरित्या समाविष्ट केले";
                            }
                            else
                            {
                                result.status = "Error";
                                result.message = "ULB Doc Name Does Not Exist";
                                result.messageMar = "ULB डॉक नाव अस्तित्वात नाही";

                            }

                        }
                        else
                        {
                            result.status = "Error";
                            result.message = "ULB Doc Sub Name already Exist";
                            result.messageMar = "ULB डॉक सब नाव आधीपासून अस्तित्वात आहे";

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


        public async Task<DocSubMasterVM> GetULBDocSubMasterAsync(int docId)
        {
            var result = new DocSubMasterVM();

            try
            {
                using (dbMain)
                {
                    result = await dbMain.DocSubMasters.Where(a => a.Id == docId).Select(a => new DocSubMasterVM
                    {
                        Id = a.Id,
                        DocId = a.DocId,
                        DocSubName = a.DocSubName,
                        DocSubDate = a.DocSubDate,
                        IsActive = a.IsActive

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

        public async Task<List<DocSubMasterVM>> GetAllULBDocSubMasterAsync()
        {
            var result = new List<DocSubMasterVM>();

            try
            {
                using (dbMain)
                {
                    result = await dbMain.DocSubMasters.Select(a => new DocSubMasterVM
                    {
                        Id = a.Id,
                        DocId = a.DocId,
                        DocSubName = a.DocSubName,
                        DocSubDate = a.DocSubDate,
                        IsActive = a.IsActive

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

        public async Task<List<DocSubMasterVM>> GetAllULBDocSubMasterByIdAsync(int docId)
        {
            var result = new List<DocSubMasterVM>();

            try
            {
                using (dbMain)
                {
                    result = await dbMain.DocSubMasters.Where(a => a.DocId == docId).Select(a => new DocSubMasterVM
                    {
                        Id = a.Id,
                        DocId = a.DocId,
                        DocSubName = a.DocSubName,
                        DocSubDate = a.DocSubDate,
                        IsActive = a.IsActive,

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


        public async Task<Result> SaveULBDocSendDetailsAsync(ULB_Doc_SendVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var ulbObj = await dbMain.ULB_Doc_Sends.Where(a => a.ULBId == obj.ULBId && a.DocSubID == obj.DocSubID).FirstOrDefaultAsync();
                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.userId))
                    {
                        if (ulbObj != null)
                        {
                            if (await dbMain.DocSubMasters.AnyAsync(a => a.Id == obj.DocSubID))
                            {
                                if (((ulbObj.DocStatus is null || ulbObj.DocStatus == false) && obj.DocStatus == true) || (ulbObj.DocStatus == true && obj.DocStatus == false))
                                {
                                    ulbObj.DocStatus = obj.DocStatus;
                                    ulbObj.DocUpdateUserId = obj.userId;
                                    ulbObj.DocUpdateDate = DateTime.Now;
                                    ulbObj.Note = obj.Note;
                                }
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB Doc Send Details Updated Successfully";
                                result.messageMar = "ULB डॉक तपशील यशस्वीरित्या बदलले";
                            }
                            else
                            {
                                result.status = "Error";
                                result.message = "ULB Doc Sub Name Does Not Exist";
                                result.messageMar = "ULB डॉक सब नाव अस्तित्वात नाही";

                            }

                            await dbMain.SaveChangesAsync();

                        }
                        else
                        {
                            if (await dbMain.DocSubMasters.AnyAsync(a => a.Id == obj.DocSubID))
                            {

                                var ulbObjData = new ULB_Doc_Send();

                                ulbObjData.ULBId = obj.ULBId;
                                ulbObjData.DocSubID = obj.DocSubID;
                                ulbObjData.DocStatus = obj.DocStatus;
                                ulbObjData.DocCreateDate = DateTime.Now;
                                ulbObjData.DocCreateUserId = obj.userId;
                                ulbObjData.Note = obj.Note;

                                //string filename = "";
                                //try
                                //{
                                //    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length-1];
                                //    filename = DateTime.Now.Ticks + extension;
                                //    var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files");
                                //    if (!Directory.Exists(pathBuilt))
                                //    {
                                //        Directory.CreateDirectory(pathBuilt);
                                //    }
                                //    var path= Path.Combine(Directory.GetCurrentDirectory(), "upload\\files",filename);
                                //    using (var stream = new FileStream(path, FileMode.Create))
                                //    {
                                //        await file.CopyToAsync(stream);
                                //    }
                                //}
                                //catch (Exception ex)
                                //{

                                //}

                                dbMain.ULB_Doc_Sends.Add(ulbObjData);
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB Doc Send Details Added Successfully";
                                result.messageMar = "ULB डॉक सेंड तपशील यशस्वीरित्या समाविष्ट केले";
                            }
                            else
                            {

                                result.status = "Error";
                                result.message = "ULB Doc Sub Name Does Not Exist";
                                result.messageMar = "ULB डॉक सब नाव अस्तित्वात नाही";
                            }


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

        //public async Task<ULB_Doc_SendVM> GetULBDocSendDetailsAsync(int ulbId)
        //{
        //    ULB_Doc_SendVM result = new ULB_Doc_SendVM();

        //    try
        //    {
        //        using (dbMain)
        //        {
        //            result = await dbMain.ULB_Doc_Sends.Where(a => a.ULBId == ulbId).Select(a => new ULB_Doc_SendVM
        //            {
        //                Id = a.Id,
        //                ULBId = a.ULBId,
        //                Agreement = a.Agreement,
        //                AgreementDate = a.AgreementDate,
        //                AgreementUserId = a.AgreementUserId,
        //                Banner = a.Banner,
        //                BannerDate = a.BannerDate,
        //                BannerUserId = a.BannerUserId,
        //                Abhipray = a.Abhipray,
        //                AbhiprayDate = a.AbhiprayDate,
        //                AbhiprayUserId = a.AbhiprayUserId,
        //                Disclaimer = a.Disclaimer,
        //                DisclaimerDate = a.DisclaimerDate,
        //                DisclaimerUserId = a.DisclaimerUserId,
        //                EntryBook = a.EntryBook,
        //                EntryBookDate = a.EntryBookDate,
        //                EntryBookUserId = a.EntryBookUserId,
        //                AgreementUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.AgreementUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                BannerUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.BannerUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                AbhiprayUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.AbhiprayUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                DisclaimerUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.DisclaimerUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                EntryBookUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.EntryBookUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //            }).FirstOrDefaultAsync();
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString(), ex);
        //        return result;

        //    }

        //}


        public async Task<Result> SaveULBDigCopyDetailsAsync(ULB_DigCopy_RecVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var ulbObj = await dbMain.ULB_DigCopy_Recs.Where(a => a.ULBId == obj.ULBId && a.DocSubID == obj.DocSubID).FirstOrDefaultAsync();
                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.userId))
                    {
                        if (ulbObj != null)
                        {
                            if (await dbMain.DocSubMasters.AnyAsync(a => a.Id == obj.DocSubID))
                            {
                                if (((ulbObj.DocStatus is null || ulbObj.DocStatus == false) && obj.DocStatus == true) || (ulbObj.DocStatus == true && obj.DocStatus == false))
                                {
                                    ulbObj.DocStatus = obj.DocStatus;
                                    ulbObj.DocUpdateUserId = obj.userId;
                                    ulbObj.DocUpdateDate = DateTime.Now;
                                    ulbObj.Note = obj.Note;
                                }
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB Doc Digital Copy Details Updated Successfully";
                                result.messageMar = "ULB डॉक डिजिटल कॉपी तपशील यशस्वीरित्या बदलले";
                            }
                            else
                            {
                                result.status = "Error";
                                result.message = "ULB Doc Sub Name Does Not Exist";
                                result.messageMar = "ULB डॉक सब नाव अस्तित्वात नाही";

                            }

                            await dbMain.SaveChangesAsync();

                        }
                        else
                        {
                            if (await dbMain.DocSubMasters.AnyAsync(a => a.Id == obj.DocSubID))
                            {

                                var ulbObjData = new ULB_DigCopy_Rec();

                                ulbObjData.ULBId = obj.ULBId;
                                ulbObjData.DocSubID = obj.DocSubID;
                                ulbObjData.DocStatus = obj.DocStatus;
                                ulbObjData.DocCreateDate = DateTime.Now;
                                ulbObjData.DocCreateUserId = obj.userId;
                                ulbObjData.Note = obj.Note;

                                dbMain.ULB_DigCopy_Recs.Add(ulbObjData);
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB Doc Digital Copy Details Added Successfully";
                                result.messageMar = "ULB डॉक डिजिटल कॉपी तपशील यशस्वीरित्या समाविष्ट केले";
                            }
                            else
                            {

                                result.status = "Error";
                                result.message = "ULB Doc Sub Name Does Not Exist";
                                result.messageMar = "ULB डॉक सब नाव अस्तित्वात नाही";
                            }


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

        //public async Task<ULB_DigCopy_RecVM> GetULBDigCopyDetailsAsync(int ulbId)
        //{
        //    ULB_DigCopy_RecVM result = new ULB_DigCopy_RecVM();

        //    try
        //    {
        //        using (dbMain)
        //        {
        //            result = await dbMain.ULB_DigCopy_Recs.Where(a => a.ULBId == ulbId).Select(a => new ULB_DigCopy_RecVM
        //            {
        //                Id = a.Id,
        //                ULBId = a.ULBId,
        //                Agreement = a.Agreement,
        //                AgreementDate = a.AgreementDate,
        //                AgreementUserId = a.AgreementUserId,
        //                Banner = a.Banner,
        //                BannerDate = a.BannerDate,
        //                BannerUserId = a.BannerUserId,
        //                Abhipray = a.Abhipray,
        //                AbhiprayDate = a.AbhiprayDate,
        //                AbhiprayUserId = a.AbhiprayUserId,
        //                Disclaimer = a.Disclaimer,
        //                DisclaimerDate = a.DisclaimerDate,
        //                DisclaimerUserId = a.DisclaimerUserId,
        //                EntryBook = a.EntryBook,
        //                EntryBookDate = a.EntryBookDate,
        //                EntryBookUserId = a.EntryBookUserId,
        //                AgreementUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.AgreementUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                BannerUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.BannerUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                AbhiprayUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.AbhiprayUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                DisclaimerUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.DisclaimerUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                EntryBookUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.EntryBookUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //            }).FirstOrDefaultAsync();
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString(), ex);
        //        return result;

        //    }

        //}

        public async Task<Result> SaveULBHardCopyDetailsAsync(ULB_HardCopy_RecVM obj)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var ulbObj = await dbMain.ULB_HardCopy_Recs.Where(a => a.ULBId == obj.ULBId && a.DocSubID == obj.DocSubID).FirstOrDefaultAsync();
                    if (await dbMain.EmployeeMasters.AnyAsync(a => a.Id == obj.userId))
                    {
                        if (ulbObj != null)
                        {
                            if (await dbMain.DocSubMasters.AnyAsync(a => a.Id == obj.DocSubID))
                            {
                                if (((ulbObj.DocStatus is null || ulbObj.DocStatus == false) && obj.DocStatus == true) || (ulbObj.DocStatus == true && obj.DocStatus == false))
                                {
                                    ulbObj.DocStatus = obj.DocStatus;
                                    ulbObj.DocUpdateUserId = obj.userId;
                                    ulbObj.DocUpdateDate = DateTime.Now;
                                    ulbObj.Note = obj.Note;
                                }
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB Doc Hard Copy Details Updated Successfully";
                                result.messageMar = "ULB डॉक हार्ड कॉपी तपशील यशस्वीरित्या बदलले";
                            }
                            else
                            {
                                result.status = "Error";
                                result.message = "ULB Doc Sub Name Does Not Exist";
                                result.messageMar = "ULB डॉक सब नाव अस्तित्वात नाही";

                            }

                            await dbMain.SaveChangesAsync();

                        }
                        else
                        {
                            if (await dbMain.DocSubMasters.AnyAsync(a => a.Id == obj.DocSubID))
                            {

                                var ulbObjData = new ULB_HardCopy_Rec();

                                ulbObjData.ULBId = obj.ULBId;
                                ulbObjData.DocSubID = obj.DocSubID;
                                ulbObjData.DocStatus = obj.DocStatus;
                                ulbObjData.DocCreateDate = DateTime.Now;
                                ulbObjData.DocCreateUserId = obj.userId;
                                ulbObjData.Note = obj.Note;

                                dbMain.ULB_HardCopy_Recs.Add(ulbObjData);
                                await dbMain.SaveChangesAsync();

                                result.status = "success";
                                result.message = "ULB Doc Hard Copy Details Added Successfully";
                                result.messageMar = "ULB डॉक हार्ड कॉपी तपशील यशस्वीरित्या समाविष्ट केले";
                            }
                            else
                            {

                                result.status = "Error";
                                result.message = "ULB Doc Sub Name Does Not Exist";
                                result.messageMar = "ULB डॉक सब नाव अस्तित्वात नाही";
                            }


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

        //public async Task<ULB_HardCopy_RecVM> GetULBHardCopyDetailsAsync(int ulbId)
        //{
        //    ULB_HardCopy_RecVM result = new ULB_HardCopy_RecVM();

        //    try
        //    {
        //        using (dbMain)
        //        {
        //            result = await dbMain.ULB_HardCopy_Recs.Where(a => a.ULBId == ulbId).Select(a => new ULB_HardCopy_RecVM
        //            {
        //                Id = a.Id,
        //                ULBId = a.ULBId,
        //                Agreement = a.Agreement,
        //                AgreementDate = a.AgreementDate,
        //                AgreementUserId = a.AgreementUserId,
        //                Banner = a.Banner,
        //                BannerDate = a.BannerDate,
        //                BannerUserId = a.BannerUserId,
        //                Abhipray = a.Abhipray,
        //                AbhiprayDate = a.AbhiprayDate,
        //                AbhiprayUserId = a.AbhiprayUserId,
        //                Disclaimer = a.Disclaimer,
        //                DisclaimerDate = a.DisclaimerDate,
        //                DisclaimerUserId = a.DisclaimerUserId,
        //                EntryBook = a.EntryBook,
        //                EntryBookDate = a.EntryBookDate,
        //                EntryBookUserId = a.EntryBookUserId,
        //                AgreementUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.AgreementUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                BannerUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.BannerUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                AbhiprayUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.AbhiprayUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                DisclaimerUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.DisclaimerUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //                EntryBookUserName = dbMain.EmployeeMasters.Where(e => e.Id == (a.EntryBookUserId ?? 0)).Select(e => e.Username).FirstOrDefault(),
        //            }).FirstOrDefaultAsync();
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString(), ex);
        //        return result;

        //    }

        //}


        public async Task<Result> SaveAddAhwalAsync(int ulbId, int docId)
        {
            Result result = new Result();
            try
            {
                using (dbMain)
                {
                    var ulbObj = await dbMain.ULB_Details.Where(a => a.Id == ulbId).FirstOrDefaultAsync();

                    var subCount = await dbMain.DocSubMasters.Where(a => a.DocId == docId).ToListAsync();

                    if (ulbObj != null && docId==3)
                    {
                        if(ulbObj.AhwalCount==null || ulbObj.AhwalCount == 0)
                        {
                            ulbObj.AhwalCount = 1;
                        }
                        int NewAhwalCount =Convert.ToInt32(ulbObj.AhwalCount)+1;
                        if (NewAhwalCount <= Convert.ToInt32(subCount.Count))
                        {
                            ulbObj.AhwalCount = NewAhwalCount;
                            await dbMain.SaveChangesAsync();
                            result.status = "success";
                            result.message = "Ahwal-"+ NewAhwalCount + " Added Successfully";
                            result.messageMar = "Ahwal-" + NewAhwalCount + " तपशील यशस्वीरित्या बदलले";
                        }
                        else
                        {
                            result.status = "Error";
                            result.message = "Not Permissison";
                            result.messageMar = "परवानगी नाही";
                        }

                    }
                    else
                    {
                        result.status = "Error";
                        result.message = "Not Permissison";
                        result.messageMar = "परवानगी नाही";

                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                result.status = "Error";
                // result.message = "Something is wrong,Try Again.. ";
                result.message = ex.Message;
                result.messageMar = "काहीतरी चुकीचे आहे, पुन्हा प्रयत्न करा..";
                return result;
            }


        }


    }
}
