using Bus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public LoggingMiddleware(RequestDelegate next, IConfiguration Configuration)
        {
            _next = next;
            _configuration = Configuration;
        }

        public async Task Invoke(HttpContext context, BusContext busContext)
        {
            if (bool.Parse(_configuration.GetSection("EnableDBLogging").Value) && context.Request.RouteValues["action"] != null)
            {
                string action = context.Request.RouteValues["action"].ToString();
                string path = context.Request.Path;

                ApiAccess apiAccess = new();
                apiAccess.Action = action;
                apiAccess.Path = path;
                apiAccess.DateAccessed = DateTime.UtcNow;

                busContext.ApiAccesses.Add(apiAccess);
                await busContext.SaveChangesAsync();
            }

            await _next(context);
        }
    }
}