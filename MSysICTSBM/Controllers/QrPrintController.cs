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
    public class QrPrintController : ControllerBase
    {
        private readonly ILogger<QrPrintController> _logger;
        private readonly IRepository objRep;
        public QrPrintController(ILogger<QrPrintController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;
        }

        [HttpPost("Save/QrPrint")]
        public async Task<Result> SaveQrPrint([FromBody] QrPrintedVM obj)
        {
            Result objResult = new Result();
            objResult = await objRep.SaveQrPrintAsync(obj);
            return objResult;
        }


        [HttpGet("Get/QrPrint")]
        public async Task<List<QrPrintedVM>> GetQrPrintDetails()
        {
            List<QrPrintedVM> objResult = new List<QrPrintedVM>();
            objResult = await objRep.GetQrPrintDetailsAsync();
            return objResult;
        }

    }
}
