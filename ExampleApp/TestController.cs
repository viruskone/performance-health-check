using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpPost]
        public async Task WorkOne(WorkModel work)
        {
            await Task.Delay(work.WaitTime);
        }

    }

}
