using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Persistence;
using Store.G02.Persistence.Data.Contexts;
using Store.G02.Services;
using Store.G02.Services.Abstractions;
using Store.G02.Services.Mapping.Products;
using Store.G02.Shard.ErrorModels;
using Store.G02.Web.Extensions;
using Store.G02.Web.Middlewares;
namespace Store.G02.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddAllServices(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddleWaresAsync();
            
            app.Run();
        }
    }
}
