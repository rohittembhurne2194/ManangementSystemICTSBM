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
    public class QrReceiveController : ControllerBase
    {

        private readonly ILogger<QrReceiveController> _logger;
        private readonly IRepository objRep;
        public QrReceiveController(ILogger<QrReceiveController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;
        }

        [HttpPost("Save/QrReceive")]
        public async Task<Result> SaveQrReceive([FromBody] QrReceiveVM obj)
        {
            Result objResult = new Result();
            objResult = await objRep.SaveQrReceiveAsync(obj);
            return objResult;
        }


        [HttpGet("GetAll/QrReceive")]
        public async Task<List<QrReceiveVM>> GetAllQrReceiveDetails()
        {
            List<QrReceiveVM> objResult = new List<QrReceiveVM>();
            objResult = await objRep.GetAllQrReceiveDetailsAsync();
            return objResult;
        }

        [HttpGet("Get/QrReceive")]
        public async Task<QrReceiveVM> GetQrReceiveDetails(int Id)
        {
            QrReceiveVM objResult = new QrReceiveVM();
            objResult = await objRep.GetQrReceiveDetailsAsync(Id);
            return objResult;
        }


    }
}
