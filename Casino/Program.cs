using Casino.API.Infrastructure.Extensions;
using Casino.API.Infrastructure.JWT;
using Casino.Application.Repositories;
using Casino.Application.Services;
using Casino.Domain.Repositories;
using Casino.Infrastructure;
using Casino.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Casino
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAuthentication();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerConfiguration();

            builder.Services.AddTokenAuthentication(builder.Configuration["JWTConfig:Secret"]!);
            builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection(nameof(JWTConfig)));

            builder.Services.AddDbContext<CasinoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
