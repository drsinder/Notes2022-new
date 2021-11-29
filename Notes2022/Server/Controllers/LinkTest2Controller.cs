using Microsoft.AspNetCore.Mvc;
using Notes2022.Server.Services;
using System.Web;

namespace Notes2022.Server.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/{uri}/{file}")]
    [ApiController]
    public class LinkTest2Controller : ControllerBase
    {
        public LinkTest2Controller()
        {
        }

        /// <summary>
        /// Gets for file existance at remote uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> Get(string uri, string file)
        {
            string urireal = HttpUtility.UrlDecode(uri);

            LinkProcessor lp = new LinkProcessor(null);
            bool test = await lp.Test2(urireal, file);
            return test;
        }
    }
}