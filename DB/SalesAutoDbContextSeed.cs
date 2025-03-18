using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SalesAuto.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB
{
    public class SalesAutoDbContextSeed
    {
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        public async Task SeedAsync(SalesAutoDbContext context, ILogger<SalesAutoDbContextSeed> logger)
        {
            if (!context.Users.Any())
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin1@gmail.com",
                    NormalizedEmail = "admin1@gmail.com",
                    PhoneNumber = "0908325345",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                user.PasswordHash = _passwordHasher.HashPassword(user, "Admin@1234");
                context.Users.Add(user);

                var user2 = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "vu.tran@matsaigon.com",
                    NormalizedEmail = "vu.tran@matsaigon.com",
                    PhoneNumber = "0908325345",
                    UserName = "vu.tran",
                    NormalizedUserName = "vu.tran",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                user2.PasswordHash = _passwordHasher.HashPassword(user2, "Vu.tran@11111");
                context.Users.Add(user2);

                var user3 = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "huy.hoang@matsaigon.com",
                    NormalizedEmail = "huy.hoang@matsaigon.com",
                    PhoneNumber = "0908325345",
                    UserName = "huy.hoang",
                    NormalizedUserName = "huy.hoang",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                user3.PasswordHash = _passwordHasher.HashPassword(user3, "huy.hoang567");
                context.Users.Add(user3);
            }
            if (!context.Roles.Any())
            {
                var role1 = new Role()
                {
                    Name = "Sale",
                    NormalizedName = "SALE",
                    MoTa="Các công việc liên quan đến sale"
                };
                context.Roles.Add(role1);
                var role2 = new Role()
                {
                    Name = "abr",
                    NormalizedName = "ABR",
                    MoTa = "Liên quan công việc đến ABR"
                };
                context.Roles.Add(role2);
            }
            await context.SaveChangesAsync();
        }
    }
}
