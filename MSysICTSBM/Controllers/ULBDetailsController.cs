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
        public async Task<ActionResult<ULB_DetailVM>> GetULBDetails(int Id)
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
        public async Task<List<ULBStatusVM>> GetULBStatus(int ulbId)
        {
            List<ULBStatusVM> objResult = new List<ULBStatusVM>();
            objResult = await objRep.GetULBStatusAsync(ulbId);
            return objResult;
        }

        [HttpGet("Get/ULBFormStatus")]
        public async Task<ULBFormStatusVM> GetULBFormStatus(int ulbId)
        {
            ULBFormStatusVM objResult = new ULBFormStatusVM();
            objResult = await objRep.GetULBFormStatusAsync(ulbId);
            return objResult;
        }
    }
}
