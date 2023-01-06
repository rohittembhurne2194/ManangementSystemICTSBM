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


        [HttpGet("GetAll/QrSent")]
        public async Task<List<QrSentVM>> GetAllQrPrintDetails()
        {
            List<QrSentVM> objResult = new List<QrSentVM>();
            objResult = await objRep.GetAllQrSentDetailsAsync();
            return objResult;
        }
        [HttpGet("Get/QrSent")]
        public async Task<QrSentVM> GetQrPrintDetails([FromHeader] int Id)
        {
            QrSentVM objResult = new QrSentVM();
            objResult = await objRep.GetQrSentDetailsAsync(Id);
            return objResult;
        }

    }
}
