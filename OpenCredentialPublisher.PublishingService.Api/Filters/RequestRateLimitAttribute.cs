using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OpenCredentialPublisher.PublishingService.Api
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestRateLimitAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public long Milliseconds { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();

            var ipAddress = context.HttpContext.Connection.RemoteIpAddress;
            var cacheKey = $"{Name}-{ipAddress}";

            DateTime expires = DateTimeOffset.UtcNow.AddMilliseconds(Milliseconds).UtcDateTime;
            
            if (!cache.TryGetValue(cacheKey, out DateTime _))
            {
                cache.Set(cacheKey, expires, DateTimeOffset.UtcNow.AddMilliseconds(Milliseconds));
            }
            else
            {
                context.Result = new ContentResult
                {
                    Content = $"You have exceeded the number of requests allowed within a given timeframe."
                };

                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.TooManyRequests; // 429
            }
        }
    }
}