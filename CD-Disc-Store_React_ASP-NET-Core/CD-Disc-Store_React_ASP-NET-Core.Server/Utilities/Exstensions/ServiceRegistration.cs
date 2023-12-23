using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exstensions
{
    public static class ServiceRegistration
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDapperContext, DapperContext>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IDiscRepository, DiscRepository>();
            services.AddScoped<IFilmRepository, FilmRepositiry>();
        }
    }
}
