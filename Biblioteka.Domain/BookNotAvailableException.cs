using System;

namespace Biblioteka.Domain
{
    public class BookNotAvailableException : InvalidOperationException
    {
        public BookNotAvailableException() : base("The book is currently unavailable.")
        {
        }
    }
}
