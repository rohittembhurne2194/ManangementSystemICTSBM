using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSysICTSBM.API.Bll.Repository.Repository;
using MSysICTSBM.API.Bll.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace MSysICTSBM.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ULBDetailsController : ControllerBase
    {
        private readonly ILogger<ULBDetailsController> _logger;
        private readonly IRepository objRep;
        public ULBDetailsController(ILogger<ULBDetailsController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;
        }


        [HttpPost("Save/ULBDetails")]
        public async Task<Result> SaveULBDetails([FromBody] ULB_DetailVM obj)
        {

            Result objResult = new Result();
            objResult = await objRep.SaveULBDetailsAsync(obj);
            return objResult;
        }


        [HttpGet("GetAll/ULBDetails")]
        public async Task<List<ULB_DetailVM>> GetAllULBDetails()
        //public async Task<List<ULB_DetailVM>> GetAllULBDetails([FromHeader] int userId)
        {
            //List<Claim> lstClaims = HttpContext.User.Claims.ToList();
            int userId = Convert.ToInt32(HttpContext.User.Claims.ToList().First(claim => claim.Type == "UserId").Value);
            List<ULB_DetailVM> objResult = new List<ULB_DetailVM>();
            objResult = await objRep.GetAllULBDetailsAsync(userId);
            return objResult;
        }

        [HttpGet("Get/ULBDetails")]
        public async Task<ActionResult<ULB_DetailVM>> GetULBDetails([FromHeader] int Id)
        {
            ULB_DetailVM objResult = new ULB_DetailVM();
            objResult = await objRep.GetULBDetailsAsync(Id);
            return Ok(objResult);

        }

        [HttpGet("GetActive/ULBDetails")]
        public async Task<List<ActiveULBVM>> GetActiveULBDetails()
        {
            List<ActiveULBVM> objResult = new List<ActiveULBVM>();
            objResult = await objRep.GetActiveULBDetailsAsync();
            return objResult;
        }

        [HttpGet("Get/ULBStatus")]
        public async Task<List<ULBStatusVM>> GetULBStatus([FromHeader] int ulbId)
        {
            List<ULBStatusVM> objResult = new List<ULBStatusVM>();
            objResult = await objRep.GetULBStatusAsync(ulbId);
            return objResult;
        }

        [HttpGet("Get/ULBDocStatus")]
        public async Task<List<ULBDocStatusVMDocData>> GetULBDocStatus([FromHeader] int ulbId, [FromHeader] int docId)
        {
            List<ULBDocStatusVMDocData> objResult = new List<ULBDocStatusVMDocData>();
            objResult = await objRep.GetULBDocStatusAsync(ulbId, docId);
            return objResult;
        }

        [HttpGet("GetAll/ULBDocStatus")]
        public async Task<List<ULBDocStatusVMNew>> GetAllULBDocStatus([FromHeader] int ulbId)
        {
            List<ULBDocStatusVMNew> objResult = new List<ULBDocStatusVMNew>();
            objResult = await objRep.GetAllULBDocStatusAsync(ulbId);
            return objResult;
        }

        [HttpGet("Get/ULBFormStatusNew")]
        public async Task<List<ULBFormStatusVMNew>> GetULBFormStatusNew([FromHeader] int ulbId)
        {
            List<ULBFormStatusVMNew> objResult = new List<ULBFormStatusVMNew>();
            objResult = await objRep.GetULBFormStatusNewAsync(ulbId);
            return objResult;
        }


        [HttpGet("Get/ULBFormStatus")]  //Not In Use
        public async Task<ULBFormStatusVM> GetULBFormStatus([FromHeader] int ulbId)
        {
            ULBFormStatusVM objResult = new ULBFormStatusVM();
            objResult = await objRep.GetULBFormStatusAsync(ulbId);
            return objResult;
        }

        [HttpPost("Save/ULBAppDetails")]
        public async Task<Result> SaveULBAppDetails([FromBody] ULB_App_StatusVM obj)
        {

            Result objResult = new Result();
            objResult = await objRep.SaveULBAppDetailsAsync(obj);
            return objResult;
        }

        [HttpGet("Get/ULBAppDetails")]
        public async Task<ULB_App_StatusVM> GetULBAppDetails([FromHeader] int ulbId)
        {

            ULB_App_StatusVM objResult = new ULB_App_StatusVM();
            objResult = await objRep.GetULBAppDetailsAsync(ulbId);
            return objResult;
        }

        [HttpPost("Save/ULBDocMasters")]
        public async Task<Result> SaveULBDocMaster([FromBody] DocMasterVM obj)
        {

            Result objResult = new Result();
            objResult = await objRep.SaveULBDocMasterAsync(obj);
            return objResult;
        }

        [HttpGet("Get/ULBDocMasters")]
        public async Task<DocMasterVM> GetULBDocMaster([FromHeader] int docId)
        {

            DocMasterVM objResult = new DocMasterVM();
            objResult = await objRep.GetULBDocMasterAsync(docId);
            return objResult;
        }

        [HttpGet("GetAll/ULBDocMasters")]
        public async Task<List<DocMasterVM>> GetAllULBDocMaster()
        {

            List<DocMasterVM> objResult = new List<DocMasterVM>();
            objResult = await objRep.GetAllULBDocMasterAsync();
            return objResult;
        }


        [HttpPost("Save/ULBDocSubMasters")]
        public async Task<Result> SaveULBDocSubMaster([FromBody] DocSubMasterVM obj)
        {

            Result objResult = new Result();
            objResult = await objRep.SaveULBDocSubMasterAsync(obj);
            return objResult;
        }

        [HttpGet("Get/ULBDocSubMasters")]
        public async Task<DocSubMasterVM> GetULBDocSubMaster([FromHeader] int Id)
        {

            DocSubMasterVM objResult = new DocSubMasterVM();
            objResult = await objRep.GetULBDocSubMasterAsync(Id);
            return objResult;
        }

        [HttpGet("GetAll/ULBDocSubMasters")]
        public async Task<List<DocSubMasterVM>> GetAllULBDocSubMaster()
        {

            List<DocSubMasterVM> objResult = new List<DocSubMasterVM>();
            objResult = await objRep.GetAllULBDocSubMasterAsync();
            return objResult;
        }

        [HttpGet("GetAll/ULBDocSubMastersById")]
        public async Task<List<DocSubMasterVM>> GetAllULBDocSubMasterById([FromHeader] int docId)
        {

            List<DocSubMasterVM> objResult = new List<DocSubMasterVM>();
            objResult = await objRep.GetAllULBDocSubMasterByIdAsync(docId);
            return objResult;
        }

        [HttpPost("Save/ULBDocSendDetails")]
        public async Task<Result> SaveULBDocDetails([FromForm] ULB_Doc_SendVM obj)
        {

            Result objResult = new Result();
            objResult = await objRep.SaveULBDocSendDetailsAsync(obj);
            return objResult;
        }

        //[HttpGet("Get/ULBDocSendDetails")]
        //public async Task<ULB_Doc_SendVM> GetULBDocDetails(int ulbId)
        //{

        //    ULB_Doc_SendVM objResult = new ULB_Doc_SendVM();
        //    objResult = await objRep.GetULBDocSendDetailsAsync(ulbId);
        //    return objResult;
        //}


        [HttpPost("Save/ULBDigCopyDetails")]
        public async Task<Result> SaveULBDigCopyDetails([FromBody] ULB_DigCopy_RecVM obj)
        {

            Result objResult = new Result();
            objResult = await objRep.SaveULBDigCopyDetailsAsync(obj);
            return objResult;
        }


        //[HttpGet("Get/ULBDigCopyDetails")]
        //public async Task<ULB_DigCopy_RecVM> GetULBDigCopyDetails(int ulbId)
        //{

        //    ULB_DigCopy_RecVM objResult = new ULB_DigCopy_RecVM();
        //    objResult = await objRep.GetULBDigCopyDetailsAsync(ulbId);
        //    return objResult;
        //}


        [HttpPost("Save/ULBHardCopyDetails")]
        public async Task<Result> SaveULBHardCopyDetails([FromBody] ULB_HardCopy_RecVM obj)
        {

            Result objResult = new Result();
            objResult = await objRep.SaveULBHardCopyDetailsAsync(obj);
            return objResult;
        }

        //[HttpGet("Get/ULBHardCopyDetails")]
        //public async Task<ULB_HardCopy_RecVM> GetULBHardCopyDetails(int ulbId)
        //{

        //    ULB_HardCopy_RecVM objResult = new ULB_HardCopy_RecVM();
        //    objResult = await objRep.GetULBHardCopyDetailsAsync(ulbId);
        //    return objResult;
        //}

        [HttpPost("Save/AddAhwal")]
        public async Task<Result> SaveAddAhwal([FromHeader] int ulbId, [FromHeader] int docId)
        {

            Result objResult = new Result();
            objResult = await objRep.SaveAddAhwalAsync(ulbId, docId);
            return objResult;
        }

        [HttpGet("File/Download")]
        public async Task<IActionResult> DownloadFile([FromHeader] string filename)
        {

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "upload\\files", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }

        [HttpPost("File/Delete")]
        public async Task<Result> DeleteFile([FromHeader] string filename)
        {
            Result objResult = new Result();
            objResult = await objRep.DeleteFileAsync(filename);
            return objResult;
        }

    }
}
