using CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exstensions;
using Microsoft.AspNetCore.Identity;

namespace CD_Disc_Store_React_ASP_NET_Core.Server
{
    public class Program
    {
        static async Task InitialDatabase(IServiceProvider host)
        {
            const string DEFAULT_ADMIN_ROLE_NAME = "Administrator";
            const string DEFAULT_EMPLOYEE_ROLE_NAME = "Employee";
            const string DEFAULT_CLIENT_ROLE_NAME = "Client";

            using (var serviceScope = host.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                var userStore = services.GetRequiredService<IUserStore<IdentityUser>>();
                var emailStore = (IUserEmailStore<IdentityUser>)userStore;

                var adminRole = await roleManager.FindByNameAsync(DEFAULT_ADMIN_ROLE_NAME);
                var employeeRole = await roleManager.FindByNameAsync(DEFAULT_EMPLOYEE_ROLE_NAME);
                var clientRole = await roleManager.FindByNameAsync(DEFAULT_CLIENT_ROLE_NAME);

                const string adminUser = "admin@host.com";
                const string adminPass = "Aa1234567890-";

                if (adminRole == null)
                {
                    var adminR = await roleManager.CreateAsync(new IdentityRole(DEFAULT_ADMIN_ROLE_NAME));
                    if (adminR.Succeeded == false)
                    {
                        throw new Exception("Error create Administrator role");
                    }
                    adminRole = await roleManager.FindByNameAsync(DEFAULT_ADMIN_ROLE_NAME);
                }

                if (employeeRole == null)
                {
                    var managerR = await roleManager.CreateAsync(new IdentityRole(DEFAULT_EMPLOYEE_ROLE_NAME));
                    if (managerR.Succeeded == false)
                    {
                        throw new Exception("Error create Manager role");
                    }
                    employeeRole = await roleManager.FindByNameAsync(DEFAULT_EMPLOYEE_ROLE_NAME);
                }

                if (clientRole == null)
                {
                    var userR = await roleManager.CreateAsync(new IdentityRole(DEFAULT_CLIENT_ROLE_NAME));
                    if (userR.Succeeded == false)
                    {
                        throw new Exception("Error create User role");
                    }
                    clientRole = await roleManager.FindByNameAsync(DEFAULT_CLIENT_ROLE_NAME);
                }

                var userAdmin = await userManager.FindByNameAsync(adminUser);
                if (userAdmin == null)
                {
                    var user = Activator.CreateInstance<IdentityUser>();

                    await userStore.SetUserNameAsync(user, adminUser, CancellationToken.None);
                    await emailStore.SetEmailAsync(user, adminUser, CancellationToken.None);

                    var result = await userManager.CreateAsync(user, adminPass);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Error create user {adminUser} with password {adminPass}");
                    }

                    var userId = await userManager.GetUserIdAsync(user);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var resultConfirm = await userManager.ConfirmEmailAsync(user, code);
                    if (!resultConfirm.Succeeded)
                    {
                        throw new Exception("");
                    }

                    userAdmin = await userManager.FindByNameAsync(adminUser);
                }

                if (!await userManager.IsInRoleAsync(userAdmin, DEFAULT_ADMIN_ROLE_NAME))
                {
                    await userManager.AddToRoleAsync(userAdmin, DEFAULT_ADMIN_ROLE_NAME);
                }
            }
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterRepositories();

            builder.Services.AddControllers();

            builder.Services.RegisterIdentity(builder.Configuration);

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

                InitialDatabase(app.Services).Wait();
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