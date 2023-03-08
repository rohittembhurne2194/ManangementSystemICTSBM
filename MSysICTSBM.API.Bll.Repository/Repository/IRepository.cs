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
        Task<Result> SaveULBAppDetailsAsync(ULB_App_StatusVM obj);
        Task<ULB_App_StatusVM> GetULBAppDetailsAsync(int ulbId);
        //Task<ULB_Doc_SendVM> GetULBDocSendDetailsAsync(int ulbId);
        //Task<Result> SaveULBDocSendDetailsAsync(ULB_Doc_SendVM obj);
        //Task<Result> SaveULBDigCopyDetailsAsync(ULB_DigCopy_RecVM obj);
        //Task<ULB_DigCopy_RecVM> GetULBDigCopyDetailsAsync(int ulbId);
        //Task<Result> SaveULBHardCopyDetailsAsync(ULB_HardCopy_RecVM obj);
        //Task<ULB_HardCopy_RecVM> GetULBHardCopyDetailsAsync(int ulbId);
        Task<Result> SaveULBDocMasterAsync(DocMasterVM obj);
        Task<DocMasterVM> GetULBDocMasterAsync(int docId);
        Task<List<DocMasterVM>> GetAllULBDocMasterAsync();
        Task<Result> SaveULBDocSubMasterAsync(DocSubMasterVM obj);
        Task<DocSubMasterVM> GetULBDocSubMasterAsync(int docId);
        Task<List<DocSubMasterVM>> GetAllULBDocSubMasterAsync();
        Task<List<DocSubMasterVM>> GetAllULBDocSubMasterByIdAsync(int docId);
        Task<Result> SaveULBDocSendDetailsAsync(ULB_Doc_SendVM obj);
        Task<Result> SaveULBDigCopyDetailsAsync(ULB_DigCopy_RecVM obj);
        Task<List<ULBDocStatusVMDocData>> GetULBDocStatusAsync(int ulbId, int docId);
        Task<Result> SaveULBHardCopyDetailsAsync(ULB_HardCopy_RecVM obj);
        Task<List<ULBDocStatusVMNew>> GetAllULBDocStatusAsync(int ulbId);
    }
}
