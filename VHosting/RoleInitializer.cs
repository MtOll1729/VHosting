using Microsoft.AspNetCore.Identity;

namespace VHosting
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            string nickname = "Knight";
            string adminEmail = "da@da.com";
            string password = "1234-asD";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("user"));
            }
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = nickname, Nickname = nickname, Password = password, AccountType=1};
                IdentityResult result = await userManager.CreateAsync(admin, password);


                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }

    }
}
