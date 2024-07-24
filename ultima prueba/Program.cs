
using Microsoft.EntityFrameworkCore;
using oracleDataAcess.Models;
using ultima_prueba.Controllers;

namespace ultima_prueba
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //conexion cadena
            var cadenaConexion = builder.Configuration.GetConnectionString("defaultConnetion");
            builder.Services.AddDbContext<ModelContext>(x =>
            x.UseOracle(
                cadenaConexion
                
                )
            );
            builder.Services.AddControllers();
            // Add services to the container
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();



            app.MapControllers();
            app.Run();
        }
    }
}
