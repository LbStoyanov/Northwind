
using Microsoft.EntityFrameworkCore;
using Northwind.API.Contracts;
using Northwind.API.Data;
using Northwind.API.Services;


namespace Northwind.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();


            builder.Services.AddDbContext<NorthwindContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindDatabase")));

            builder.Services.AddScoped<INorthwindContext>(provider => provider.GetRequiredService<NorthwindContext>());
            builder.Services.AddScoped<ICustomerService,CustomerService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(policy => policy
                .AllowAnyOrigin() // Or better: .WithOrigins("https://localhost:xxxx") if you know the frontend URL
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
