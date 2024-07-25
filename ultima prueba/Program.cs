
using Microsoft.EntityFrameworkCore;
using oracleDataAcess.Models;
using ultima_prueba.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            /////////////////////////////////////////////jwt///////////////////
            builder.Configuration.AddJsonFile("appsettings.json");
            var secretKey = builder.Configuration.GetSection("JWT").GetSection("Secretkey").ToString();
            
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication(config => {

                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config => {
                config.RequireHttpsMetadata = false;
                config.SaveToken = false;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


         

          
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////7
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
