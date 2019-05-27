using System;
using Biblioteka.Domain.Entities;

namespace Biblioteka.Application
{
    public interface IUserAuthenticationService
    {
        User User { get; }

        User Login(string login, string password);
    }
}
