using DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesAuto.Models.Entities;
using SalesAuto.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesAuto.Api.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly SalesAutoDbContext context;

        public UserRepo(SalesAutoDbContext context)
        {
            this.context = context;
        }
        public async Task<List<User>> GetUserList()
        {
            return await context.Users.ToListAsync();
        }

        public async Task Save(UserVM userVM)
        {
            var found = await context.Users.FirstOrDefaultAsync(x => x.UserName == userVM.UserName);
            if (found!=null)
            {
                found.Email = userVM.Email;
                found.NormalizedEmail = userVM.NormalizedEmail;
                found.PhoneNumber = userVM.PhoneNumber;
                found.NormalizedUserName = userVM.NormalizedUserName;                
                found.PasswordHash = _passwordHasher.HashPassword(found, userVM.Password);
            }
            else
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = userVM.Email,
                    NormalizedEmail = userVM.NormalizedEmail,
                    PhoneNumber = userVM.PhoneNumber,
                    UserName = userVM.UserName,
                    NormalizedUserName = userVM.NormalizedUserName,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                user.PasswordHash = _passwordHasher.HashPassword(user, userVM.Password);
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
                    
        }
    }
}
 