using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;

namespace TodoWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public TestController(IMemoryCache cache)
        {
            _memoryCache = cache;
        }

        [HttpGet("test")]
        //[OutputCache(Duration = 3000)]
        public async Task<IActionResult> Test()
        {
            if(_memoryCache.TryGetValue<string>("cachedData", out var cachedData))
                return Ok(cachedData);


            await Task.Delay(3000);

            var data = "it works";

            _memoryCache.Set("cachedData", data, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(10),
            });

            //_memoryCache.Remove(key)

            return Ok(data);
        }
    }
}
