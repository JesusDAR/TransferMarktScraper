using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace TransferMarktScraper.WebApi.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/errors")]
    [EnableCors("CorsPolicy")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                string routeWhereExceptionOccurred = exceptionFeature.Path;
                Exception exceptionThatOccurred = exceptionFeature.Error;
                Log.Error("{0} - {1}", routeWhereExceptionOccurred, exceptionThatOccurred);
                return Problem(
                    detail: exceptionFeature.Error.StackTrace,
                    title: exceptionFeature.Error.Message);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
