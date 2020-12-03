using Microsoft.AspNetCore.Identity;

namespace SimpleAppAspNetIdentity.Data
{
    public static class DbInitializer
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {

            if (userManager.FindByNameAsync("bob").Result == null)
            {
                IdentityUser user = new IdentityUser()
                {
                    UserName = "bob",
                    Email = "bob@localhost",
                    EmailConfirmed = true,
                    //Name = "Bob"
                };
                
                IdentityResult result = userManager.CreateAsync(
                    user, "P@ssw0rd!").Result;
            }

            if (userManager.FindByNameAsync("alice").Result == null)
            {
                IdentityUser user = new IdentityUser()
                {
                    UserName = "alice",
                    Email = "alice@localhost",
                    EmailConfirmed = true,
                    //Name = "Alice"
                };

                IdentityResult result = userManager.CreateAsync(
                    user, "P@ssw0rd!").Result;
            }

        }
    }
}
