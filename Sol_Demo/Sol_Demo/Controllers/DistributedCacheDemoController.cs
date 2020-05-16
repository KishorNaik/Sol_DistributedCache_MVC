using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Sol_Demo.Models;

namespace Sol_Demo.Controllers
{
    public class DistributedCacheDemoController : Controller
    {
        private readonly IDistributedCache distributedCache = null;

        public DistributedCacheDemoController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
            CacheM = new CacheModel();
        }

        public CacheModel CacheM { get; set; }

        public async Task<IActionResult> Index()
        {
            var currentDateTimeObject = DateTime.Now.ToString();

           var cacheDateTime=await  this.distributedCache.GetStringAsync("CacheDateTime");
            if (cacheDateTime == null)
            {
                var distributeCacheEntryOption = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
                };
                await this.distributedCache.SetStringAsync("CacheDateTime", currentDateTimeObject, distributeCacheEntryOption);
                cacheDateTime = await this.distributedCache.GetStringAsync("CacheDateTime");
            }

            CacheM.CurrentDateTime = currentDateTimeObject;
            CacheM.CacheDateTime = cacheDateTime;

            return View(CacheM);
        }
    }
}