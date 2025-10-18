using Microsoft.AspNetCore.Identity;
using NetIdentity.Models;
using System.Security.Claims;

namespace NetIdentity.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Usuario" };
            foreach (var roleName in roleNames)
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));

            if (await userManager.FindByEmailAsync("admin@test.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    FechaNacimiento = DateTime.Now.AddYears(-30),
                    NombreCompleto = "Administrador Sistema",
                    EmailConfirmed = true,
                    genero = "Femenino"
                };
                var r = await userManager.CreateAsync(adminUser, "Admin123!");
                if (r.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.AddClaimAsync(adminUser, new Claim("FechaNacimiento", adminUser.FechaNacimiento.ToString("yyyy-MM-dd")));
                    await userManager.AddClaimAsync(adminUser, new Claim("genero", adminUser.genero));
                }
            }

            if (await userManager.FindByEmailAsync("menor@test.com") == null)
            {
                var userMenor = new ApplicationUser
                {
                    UserName = "menor@test.com",
                    Email = "menor@test.com",
                    FechaNacimiento = DateTime.Now.AddYears(-15),
                    NombreCompleto = "Juan Menor",
                    EmailConfirmed = true,
                    genero = "Femenino"
                };
                var r = await userManager.CreateAsync(userMenor, "Menor123!");
                if (r.Succeeded)
                {
                    await userManager.AddToRoleAsync(userMenor, "Usuario");
                    await userManager.AddClaimAsync(userMenor, new Claim("FechaNacimiento", userMenor.FechaNacimiento.ToString("yyyy-MM-dd")));
                    await userManager.AddClaimAsync(userMenor, new Claim("genero", userMenor.genero));
                }
            }

            if (await userManager.FindByEmailAsync("mayor@test.com") == null)
            {
                var userMayor = new ApplicationUser
                {
                    UserName = "mayor@test.com",
                    Email = "mayor@test.com",
                    FechaNacimiento = DateTime.Now.AddYears(-25),
                    NombreCompleto = "María Mayor",
                    EmailConfirmed = true,
                    genero = "Masculino"
                };
                var r = await userManager.CreateAsync(userMayor, "Mayor123!");
                if (r.Succeeded)
                {
                    await userManager.AddToRoleAsync(userMayor, "Usuario");
                    await userManager.AddClaimAsync(userMayor, new Claim("FechaNacimiento", userMayor.FechaNacimiento.ToString("yyyy-MM-dd")));
                    await userManager.AddClaimAsync(userMayor, new Claim("genero", userMayor.genero));
                }
            }
        }
    }
}
