global using CD_Disc_Store_React_ASP_NET_Core.Server.ViewModels;
global using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
global using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Options;
global using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services;
global using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions;
global using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exstensions;

namespace CD_Disc_Store_React_ASP_NET_Core.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterRepositories();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClientProjectOrigin",
                    builder => builder.WithOrigins("https://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            builder.Services.RegisterIdentity(builder.Configuration);

            builder.Services.ConfigStorageOptions(builder.Configuration);
            builder.Services.AddSingleton<ICloudStorage, GoogleCloudStorage>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowClientProjectOrigin");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
