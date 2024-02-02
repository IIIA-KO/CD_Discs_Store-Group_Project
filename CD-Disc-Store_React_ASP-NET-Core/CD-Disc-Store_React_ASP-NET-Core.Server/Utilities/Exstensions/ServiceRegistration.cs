using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Models;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Contexts;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Options;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories;
using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Processors;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Interfaces;
using CD_Disc_Store_React_ASP_NET_Core.Server.Data.Repositories.Implementations;

namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exstensions
{
    public static class ServiceRegistration
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IDapperContext, DapperContext>();

            services.AddScoped<ProcessableViewModelProcessor<Client>, ClientProcessor>();
            services.AddScoped<ProcessableViewModelProcessor<Film>, FilmProcessor>();
            services.AddScoped<ProcessableViewModelProcessor<Music>, MusicProcessor>();
            services.AddScoped<ProcessableViewModelProcessor<Disc>, DiscProcessor>();
            services.AddScoped<ProcessableViewModelProcessor<Order>, OrderProcessor>();
            services.AddScoped<ProcessableViewModelProcessor<OrderItem>, OrderItemProcessor>();
            services.AddScoped<ProcessableViewModelProcessor<OperationLog>, OperationLogProcessor>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOperationLogRepository, OperationLogRepository>();
            services.AddScoped<IMusicRepository, MusicRepository>();
            services.AddScoped<IDiscRepository, DiscRepository>();
            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();


            services.AddTransient<UserManager<IdentityUser>>();
            services.AddTransient<SignInManager<IdentityUser>>();
            services.AddTransient<RoleManager<IdentityRole>>();

            services.AddScoped<IUserStore<IdentityUser>, UserStore<IdentityUser, IdentityRole, ApplicationDbContext, string>>();
            services.AddScoped<IUserEmailStore<IdentityUser>, UserStore<IdentityUser, IdentityRole, ApplicationDbContext, string>>();
        }

        public static void RegisterIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication();
        }

        public static void ConfigStorageOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudUrls = configuration.GetSection("URLs").Get<CloudUrls>();
            var defaultImageNames = configuration.GetSection("DefaultImageStorageNames").Get<DefaultImageNames>();

            var storageOptions = new StorageOptions(defaultImageNames, cloudUrls);

            services.AddSingleton(storageOptions);
        }
    }
}
