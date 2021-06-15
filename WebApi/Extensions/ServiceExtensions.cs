using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggerService;
namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {

            //Instead of the AllowAnyOrigin() method which allows requests from any source, we could use the
            //    WithOrigins("http://www.something.com") which will allow requests just from the specified source.
            //    Also, instead of AllowAnyMethod() that allows all HTTP methods,  we can use WithMethods("POST", "GET")
            //    that will allow only specified HTTP methods.Furthermore, we can make the same changes for the AllowAnyHeader() method 
            //    by using, for example, the WithHeaders("accept", "content-type") method to allow only specified headers.
          services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
