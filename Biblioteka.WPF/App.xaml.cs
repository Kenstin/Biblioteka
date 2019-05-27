using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Biblioteka.Domain.Entities;
using Biblioteka.Persistence;
using Biblioteka.WPF.ViewModels;
using Biblioteka.WPF.Views;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using Splat;

namespace Biblioteka.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

            var builder = new DbContextOptionsBuilder<LibraryDbContext>();
            builder.UseSqlite($"Data Source={Environment.CurrentDirectory}\\database.db;");
            Locator.CurrentMutable.RegisterLazySingleton(() => new LibraryDbContext(builder.Options));
            Locator.CurrentMutable.RegisterLazySingleton<IUserService>(() => new UserService());


            var dbContext = Locator.Current.GetService<LibraryDbContext>();
            dbContext.Database.Migrate();
            if (!dbContext.Users.Any())
            {
                dbContext.Add(new User("jazda", string.Join("",
                    (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes("jazda")))
                    .Select(x => x.ToString("x2")).ToArray())));

                dbContext.Add(new User("drugi", string.Join("",
                    (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes("zaq1@WSX")))
                    .Select(x => x.ToString("x2")).ToArray())));

                dbContext.SaveChanges();
            }
        }
    }
}
