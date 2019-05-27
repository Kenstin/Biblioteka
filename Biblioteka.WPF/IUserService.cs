using System.Threading.Tasks;
using Biblioteka.Domain.Entities;

namespace Biblioteka.WPF
{
    public interface IUserService
    {
        User CurrentUser { get; }
        Task<User> TryLogin(string username, string password);
    }
}