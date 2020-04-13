using Microsoft.AspNetCore.Mvc;

namespace Synuit.Toolkit.SignalR.Test.Controllers
{
    [Route("api/[controller]")]
    public class AboutController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "This is \"Synuit.Toolkit.SignalR.Test\" service - a sample of SignalR usage";
        }
    }
}
