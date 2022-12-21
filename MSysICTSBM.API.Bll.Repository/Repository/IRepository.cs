using MSysICTSBM.API.Bll.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSysICTSBM.API.Bll.Repository.Repository
{
    public interface IRepository
    {
        Task<SBUser> CheckUserLoginAsync(SBUser obj);
        Task<SBUser> CheckUserLoginForNormalAsync(SBUser obj);
        Task<List<ActiveULBVM>> GetActiveULBDetailsAsync();
        Task<List<QrPrintedVM>> GetAllQrPrintDetailsAsync();
        Task<List<QrSentVM>> GetAllQrSentDetailsAsync();
        Task<QrPrintedVM> GetQrPrintDetailsAsync(int Id);
        Task<QrSentVM> GetQrSentDetailsAsync(int Id);
        Task<List<ULB_DetailVM>> GetAllULBDetailsAsync(int userId);
        Task<string> LoginAsync(int AppId);
        Task<Result> SaveQrPrintAsync(QrPrintedVM obj);
        Task<Result> SaveQrSentAsync(QrSentVM obj);
        Task<Result> SaveULBDetailsAsync(ULB_DetailVM obj);
        Task<Result> SaveUserAsync(EmployeeMasterVM obj);
        Task<ULB_DetailVM> GetULBDetailsAsync(int Id);
        Task<List<ULBStatusVM>> GetULBStatusAsync(int ulbId);
        Task<Result> SaveQrReceiveAsync(QrReceiveVM obj);
        Task<List<QrReceiveVM>> GetAllQrReceiveDetailsAsync();
        Task<QrReceiveVM> GetQrReceiveDetailsAsync(int Id);
        Task<ULBFormStatusVM> GetULBFormStatusAsync(int ulbId);
    }
}
