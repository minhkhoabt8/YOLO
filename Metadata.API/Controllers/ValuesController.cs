using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLib.ResponseWrapper;

namespace Metadata.API.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Get documents
        /// </summary>
        /// <returns></returns>
        [HttpGet("acc")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> QueryDocumentAsync( string query)
        {
            throw new NotImplementedException();
        }


    }
}
