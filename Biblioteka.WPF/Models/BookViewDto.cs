using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteka.Domain.Entities;

namespace Biblioteka.WPF.Models
{
    public class BookViewDto
    {
        public BookViewDto(Book book) //should use automapper instead
        {
            Id = book.Id;
            Title = book.Title;
            Author = book.Author;
            Publisher = book.Publisher;
            YearPublished = book.YearPublished;
            IsAvailable = book.IsAvailable;
        }

        public Guid Id { get; }
        
        public string Title { get; }

        public string Author { get; }

        public string Publisher { get; }

        public int YearPublished { get; }

        public bool IsAvailable { get; }
    }
}
