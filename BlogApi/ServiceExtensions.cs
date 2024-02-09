using BlogApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogApi
{
    public static  class ServiceExtensions
    {


            public static void ConfigureCors(this IServiceCollection services) =>
              services.AddCors(options =>
              {
                  options.AddPolicy("CorsPolicy", builder =>
                  builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader());
              });

            public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
            {
                var connectionString = configuration.GetSection("ConnectionString")["DefaultConn"];
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            }


            public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
            {


               



            }




        
    }
}
