using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSysICTSBM.API.Bll.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSysICTSBM.Controllers
{
    [Authorize]
    [Route("api/Account")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IRepository objRep;

        public LoginController(ILogger<LoginController> logger, IRepository repository)
        {
            _logger = logger;
            objRep = repository;
        }


        [HttpPost("getToken")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromHeader] int AppId)
        {
            var result = await objRep.LoginAsync(AppId);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
