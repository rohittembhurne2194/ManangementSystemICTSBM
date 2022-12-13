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
        Task<List<QrPrintedVM>> GetQrPrintDetailsAsync();
        Task<List<QrSentVM>> GetQrSentDetailsAsync();
        Task<List<ULB_DetailVM>> GetULBDetailsAsync();
        Task<string> LoginAsync(int AppId);
        Task<Result> SaveQrPrintAsync(QrPrintedVM obj);
        Task<Result> SaveQrSentAsync(QrSentVM obj);
        Task<Result> SaveULBDetailsAsync(ULB_DetailVM obj);
        Task<Result> SaveUserAsync(EmployeeMasterVM obj);
    }
}
