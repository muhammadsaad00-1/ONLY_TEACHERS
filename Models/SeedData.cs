using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OT.Areas.Identity.Data;
using OT.Data;
using System.Drawing.Printing;

namespace OT.Models
{
    [Authorize]
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new OTContext(
                serviceProvider.GetRequiredService<DbContextOptions<OTContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@contoso.com");
                await EnsureRole(serviceProvider, adminID, "Admin");

                // allowed user can create and edit Posts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@contoso.com");
                await EnsureRole(serviceProvider, managerID, "Manager");

                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<OTUser>>();

            var user = await userManager.FindByNameAsync(UserName) ?? null;
            if (user == null)
            {
                user = new OTUser
                {
                    FirstName = "Taaha",
                    LastName = "Rauf",

                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<OTUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        public static void SeedDB(OTContext context, string adminID)
        {
            if (context.Post.Any())
            {
                
                return;   // DB has been seeded
            }
            context.Post.AddRange(
                            new Post
                            {
                                Title = "This is My first post",
                                PostDate = DateTime.Now,
                                content = "Horny teachers nearby",
                                Price = 7.99M,
                                subscribers = 0,
                                Status=ContactStatus.Approved,
                                OwnerID = adminID
                            },
                            new Post
                            {
                                Title = "This is My Second post",
                                PostDate = DateTime.Now,
                                content = "Pay to get exclusive content",
                                Price = 7.99M,
                                subscribers = 0,
                                 Status = ContactStatus.Approved,
                                OwnerID = adminID
                            },
                            new Post
                            {
                                Title = "This is My Third post",
                                PostDate = DateTime.Now,
                                content = "Physics lectures",
                                Price = 7.99M,
                                subscribers = 0,
                                 Status = ContactStatus.Approved,
                                OwnerID = adminID
                            },
                            new Post
                            {
                                Title = "This is My Fourth post",
                                PostDate = DateTime.Now,
                                content = "DB Course by Ishaq Raza",
                                Price = 7.99M,
                                subscribers = 0,
                                 Status = ContactStatus.Approved,
                                OwnerID = adminID
                            }
                        );
        }
    }
}
