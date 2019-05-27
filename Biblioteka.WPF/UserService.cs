using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Biblioteka.Domain.Entities;
using Biblioteka.Persistence;
using Biblioteka.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;
using Splat;

namespace Biblioteka.WPF
{
    public class UserService : IUserService
    {
        private readonly LibraryDbContext dbContext;

        public User CurrentUser { get; private set; }
        public UserService(LibraryDbContext dbContext = null)
        {
            this.dbContext = dbContext ?? Locator.Current.GetService<LibraryDbContext>();
        }

        public async Task<User> TryLogin(string username, string password)
        {
            var dbUser = await dbContext.Users.Where(user => user.Username == username).SingleAsync();
            if (dbUser != null && dbUser.HashedPassword == string.Join("",
                    (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("x2")).ToArray())))
            {
                CurrentUser = dbUser;
                return dbUser;
            }
            else
            {
                return null;
            }

        }
    }
}
