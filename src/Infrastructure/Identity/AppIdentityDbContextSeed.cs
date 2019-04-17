using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Microsoft.ESportShop.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "nick.mitselos@gmail.com", Email = "nick.mitselos@gmail.com" };
            await userManager.CreateAsync(defaultUser, "Pass@word1");
        }
    }
}
