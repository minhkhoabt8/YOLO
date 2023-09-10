
using Auth.Infracstructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Auth.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISampleServices _sampleServices;

    public AuthController(ISampleServices sampleServices)
    {
        _sampleServices = sampleServices;
    }


    /// <summary>
    /// Get Sample
    /// </summary>
    /// <returns></returns>
    [HttpGet("sample")]
    public async Task<IActionResult> GetSample()
    {
        
        var result = _sampleServices.GetSampleInfo();

        return Ok(result);
    }


}