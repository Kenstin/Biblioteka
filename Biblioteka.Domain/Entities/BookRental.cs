using System;
using Biblioteka.Domain.SeedWork;

namespace Biblioteka.Domain.Entities
{
    public class BookRental : Entity
    {
        public BookRental(User user, Book book, DateTime rentDate, DateTime returnDate)
        {
            User = user ?? throw new NullReferenceException(nameof(user));
            Book = book ?? throw new NullReferenceException(nameof(book));
            RentDate = rentDate;
            ReturnDate = returnDate;

        }

        private BookRental()
        {}

        public Book Book { get; private set; }

        public User User { get; private set; }

        public DateTime RentDate { get; private set; }

        public DateTime ReturnDate { get; private set; }
    }
}
