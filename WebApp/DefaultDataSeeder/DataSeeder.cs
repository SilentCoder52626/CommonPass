using DomainModule.Entity;
using DomainModule.ServiceInterface;
using InfrastructureModule.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp.DefaultDataSeeder
{
    public class DataSeeder
    {

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();

                var RoleSuperAdmin = User.TypeSuperAdmin;
                var RoleGeneral = User.TypeGeneral;

                var Permissions = new List<string>()
                {
                    "User-ResetPassword",
                    "Profile-View",
                    "User-Update",
                    "Accounts-View",
                    "Accounts-AddOrUpdate",
                    "Accounts-DecryptPassword",
                    "Accounts-Export"
                };

                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roleService = serviceScope.ServiceProvider.GetRequiredService<RoleServiceInterface>();

                await EnsureRoleExistsAsync(roleManager, RoleSuperAdmin);
                await EnsureRoleExistsAsync(roleManager, RoleGeneral);
                await roleService.AssignPermissionInBulk(RoleGeneral, Permissions);

                // Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                await EnsureAdminUserExistsAsync(userManager, RoleSuperAdmin);
            }
        }

        private static async Task EnsureRoleExistsAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task EnsureAdminUserExistsAsync(UserManager<User> userManager, string roleSuperAdmin)
        {
            string adminUserEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

            if (adminUser == null)
            {
                var newAdminUser = new User("admin", "admin", adminUserEmail, User.TypeSuperAdmin)
                {
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(newAdminUser, "admin");
                await userManager.AddToRoleAsync(newAdminUser, roleSuperAdmin);
            }
        }

    }
}
