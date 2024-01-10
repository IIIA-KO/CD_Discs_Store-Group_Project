using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exstensions;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services.Implementations;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CD_Disc_Store_React_ASP_NET_Core.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterRepositories();

            builder.Services.AddControllers();

            builder.Services.RegisterIdentity(builder.Configuration);

            builder.Services.AddSingleton<ICloudStorage, GoogleCloudStorage>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.MapIdentityApi<IdentityUser>();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}