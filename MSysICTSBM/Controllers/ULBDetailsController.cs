using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSysICTSBM.API.Bll.Repository.Repository;
using MSysICTSBM.API.Bll.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("Get/ULBDetails")]
        public async Task<List<ULB_DetailVM>> GetULBDetails()
        {
            List<ULB_DetailVM> objResult = new List<ULB_DetailVM>();
            objResult = await objRep.GetULBDetailsAsync();
            return objResult;
        }
    }
}
