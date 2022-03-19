﻿using DataAccess.Model;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using System.Security.Claims;

namespace Server
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<AspNetIdentityDbContext>(
                options => options.UseSqlServer(connectionString)
            );

            services
                .AddIdentity<MyUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
            services.AddOperationalDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                        );
                }
            );
            services.AddConfigurationDbContext(
                options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(
                            connectionString,
                            sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName)
                        );
                }
            );

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();

            EnsureSeedData(context);

            var ctx = scope.ServiceProvider.GetService<AspNetIdentityDbContext>();
            ctx.Database.Migrate();
            EnsureRoles(scope);
            EnsureUsers(scope);
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            UserManager<MyUser> userMgr = scope.ServiceProvider.GetRequiredService<UserManager<MyUser>>();

            MyUser admin = userMgr.FindByNameAsync("admin").Result;
            MyUser john = userMgr.FindByNameAsync("customer").Result;
            if (admin == null)
            {
                admin = new MyUser
                {
                    UserName = "admin",
                    Email = "admin@metashop.com",
                    EmailConfirmed = true,
                    FirstName = "admin",
                    LastName = "admin",
                    Line1 = " Etown 1, Level 3, 364 Cong Hoa Street Tan Binh District",
                    Province = "Ho Chi Minh city",
                    Country = "Viet Nam",
                    PhoneNumber = "0123456789",
                    PhoneNumberConfirmed = true
                };
                IdentityResult result = userMgr.CreateAsync(admin, "aduvip").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddToRoleAsync(admin, "Admin").Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result =
                    userMgr.AddClaimsAsync(
                        admin,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "admin"),
                            new Claim(JwtClaimTypes.GivenName, "admin"),
                            new Claim(JwtClaimTypes.FamilyName, "admin"),
                            new Claim(JwtClaimTypes.Email, admin.Email),
                            new Claim(JwtClaimTypes.Role, "Admin")
                        }
                    ).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }

            if (john == null)
            {
                john = new MyUser
                {
                    UserName = "customer",
                    Email = "customer@metashop.com",
                    EmailConfirmed = true,
                    FirstName = "jhonny",
                    LastName = "D",
                    Line1 = " Etown 2, Level 5, 364 Cong Hoa Street Tan Binh District",
                    Province = "Ho Chi Minh city",
                    Country = "Viet Nam",
                    PhoneNumber = "0123456755",
                    PhoneNumberConfirmed = true
                };
                IdentityResult result = userMgr.CreateAsync(john, "aduvip").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddToRoleAsync(john, "Customer").Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result =
                    userMgr.AddClaimsAsync(
                        john,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, john.FirstName),
                            new Claim(JwtClaimTypes.GivenName, john.FirstName),
                            new Claim(JwtClaimTypes.FamilyName, john.LastName),
                            new Claim(JwtClaimTypes.Email, john.Email),
                            new Claim(JwtClaimTypes.Role, "Customer")
                        }
                    ).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static void EnsureRoles(IServiceScope scope)
        {
            RoleManager<IdentityRole> roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IdentityRole admin = roleMgr.FindByNameAsync("Admin").Result;
            IdentityRole customer = roleMgr.FindByNameAsync("Customer").Result;
            if (admin == null)
            {
                admin = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "admin"
                };
                var result = roleMgr.CreateAsync(admin).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
            if (customer == null)
            {
                customer = new IdentityRole
                {
                    Name = "Customer",
                    NormalizedName = "customer"
                };
                var result = roleMgr.CreateAsync(customer).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            /*if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }*/
        }
    }
}
