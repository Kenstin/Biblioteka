using System;
using Biblioteka.Domain.SeedWork;

namespace Biblioteka.Domain.Entities
{
    public class Book : Entity, IAggregateRoot
    {
        public Book(string author, string title, string publisher, int yearPublished, bool isAvailable)
        {
            Author = author ?? throw new NullReferenceException(nameof(author));
            Title = title ?? throw new NullReferenceException(nameof(author));
            Publisher = publisher ?? throw new NullReferenceException(nameof(publisher));
            YearPublished = yearPublished;
            IsAvailable = isAvailable;
        }

        public string Author { get; private set; }

        public string Title { get; private set; }

        public string Publisher { get; private set; }

        public int YearPublished { get; private set; }

        public bool IsAvailable { get; private set; }

        public BookRental Rental { get; private set; }

        public BookRental RentBook(User user)
        {
            if (!IsAvailable || Rental != null)
            {
                throw new BookNotAvailableException();
            }

            var rental = new BookRental(user, this, DateTime.Now, DateTime.Now.AddDays(7)); //we really shouldn't be using DateTime.Now
            Rental = rental;
            return rental;
        }

        public void ReturnBook()
        {
            Rental = null;
        }
    }
}
