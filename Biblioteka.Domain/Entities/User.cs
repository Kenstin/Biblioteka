using System;
using Biblioteka.Domain.SeedWork;

namespace Biblioteka.Domain.Entities
{
    public class User : Entity, IAggregateRoot
    {
        public User(string username, string hashedPassword)
        {
            Username = username ?? throw new NullReferenceException(nameof(username));
            HashedPassword = hashedPassword ?? throw new NullReferenceException(nameof(hashedPassword));
        }

        public string Username { get; }

        public string HashedPassword { get; }
    }
}
