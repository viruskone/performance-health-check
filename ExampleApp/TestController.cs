using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        public async Task WorkOne([FromQuery] int waitTime)
        {
            await Task.Delay(waitTime);
        }

    }
}
