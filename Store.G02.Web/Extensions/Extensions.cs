using Microsoft.AspNetCore.Mvc;
using Store.G02.Domain.Contracts;
using Store.G02.Persistence;
using Store.G02.Services;
using Store.G02.Shard.ErrorModels;
using Store.G02.Web.Middlewares;

namespace Store.G02.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebServices();

            services.AddApplicationServices(configuration);

            services.AddInfrastructureServices(configuration);

            services.ConfigureApiBehaviorOptions();

            return services;
        }

        private static IServiceCollection ConfigureApiBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(M => M.Value.Errors.Any())
                        .Select(M => new ValidationError()
                        {
                            Field = M.Key,
                            Errors = M.Value.Errors.Select(E => E.ErrorMessage)
                        }).ToList();

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }

        private static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static async Task<WebApplication> ConfigureMiddleWaresAsync(this WebApplication app)
        {
            // ASK From CLR
            #region Initialize Db
            await app.SeedData();

            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseGlobalErrorHandling();

            // Serve static files from wwwroot (so /images/... works)
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            return app;
        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            return app;
        }

        private static async Task<WebApplication> SeedData(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Ask CLR To Create Object From IDbInitializer
            await dbInitializer.InitializeAsync();
            return app;
        }
    }
}
