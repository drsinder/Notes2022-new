using Microsoft.AspNetCore.Mvc;
using Notes2022.Shared;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        public AboutController()
        {
        }

        /// <summary>
        /// Gets the data for the About page.  Not much to it.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<AboutModel> Get()
        {
            AboutModel model = new AboutModel
            {
                PrimeAdminName = Globals.PrimeAdminName,
                PrimeAdminEmail = Globals.PrimeAdminEmail,
                StartupDateTime = Globals.StartupDateTime
            };

            return model;
        }
    }
}
