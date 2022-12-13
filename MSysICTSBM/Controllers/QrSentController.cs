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
    public class QrSentController : ControllerBase
    {
        private readonly ILogger<QrSentController> _logger;
        private readonly IRepository objRep;
        public QrSentController(ILogger<QrSentController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;
        }

        [HttpPost("Save/QrSent")]
        public async Task<Result> SaveQrSent([FromBody] QrSentVM obj)
        {
            Result objResult = new Result();
            objResult = await objRep.SaveQrSentAsync(obj);
            return objResult;
        }


        [HttpGet("Get/QrSent")]
        public async Task<List<QrSentVM>> GetQrPrintDetails()
        {
            List<QrSentVM> objResult = new List<QrSentVM>();
            objResult = await objRep.GetQrSentDetailsAsync();
            return objResult;
        }


    }
}
