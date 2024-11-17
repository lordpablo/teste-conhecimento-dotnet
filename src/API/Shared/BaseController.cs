using Microsoft.AspNetCore.Mvc;
using SampleTest.Resources.Features;

namespace SampleTeste.API.Shared
{
    [ApiController]
    [Route("[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        protected BaseController()
        {

        }
        protected IActionResult ApiResult(Result result)
        {
            return Ok(result);
        }
    }
}