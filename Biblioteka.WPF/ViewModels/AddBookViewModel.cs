using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using Biblioteka.Domain.Entities;
using Biblioteka.Persistence;
using Biblioteka.WPF.Validators;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Biblioteka.WPF.ViewModels
{
    public class AddBookViewModel : ValidatedReactiveObject<AddBookViewModel, AddBookViewModelValidator>
    {
        private readonly LibraryDbContext libraryDbContext;

        [Reactive] public string Author { get; set; }

        public string AuthorErrors { [ObservableAsProperty] get; }

        [Reactive] public string Title { get; set; }

        public string TitleErrors { [ObservableAsProperty] get; }

        [Reactive] public string Publisher { get; set; }

        public string PublisherErrors { [ObservableAsProperty] get; }

        [Reactive] public int? YearPublished { get; set; }
        public string YearPublishedErrors { [ObservableAsProperty] get; }

        [Reactive] public bool Available { get; set; }

        public string Errors { [ObservableAsProperty] get; }

        public Interaction<Unit, Unit> BookAdded { get; }

        public ReactiveCommand<Unit, Book> AddBook { get; }

        public AddBookViewModel(LibraryDbContext dbContext = null)
        {
            libraryDbContext = dbContext ?? Locator.Current.GetService<LibraryDbContext>();

            Setup(vm => vm.Author, vm => vm.AuthorErrors);
            Setup(vm => vm.Publisher, vm => vm.PublisherErrors);
            Setup(vm => vm.Publisher, vm => vm.PublisherErrors);
            Setup(vm => vm.YearPublished, vm => vm.YearPublishedErrors);
            Setup(vm => vm.Title, vm => vm.TitleErrors);

            this.WhenAnyValue(vm => vm.AuthorErrors, vm => vm.PublisherErrors, vm => vm.TitleErrors,
                    vm => vm.YearPublishedErrors).Select(tuple => string.Join("\n", tuple))
                .ToPropertyEx(this, vm => vm.Errors);

            BookAdded = new Interaction<Unit, Unit>();
            AddBook = ReactiveCommand.CreateFromTask(async () =>
            {
                var book = new Book(Author, Title, Publisher, YearPublished.GetValueOrDefault(), Available);
                var addedBook = await libraryDbContext.AddAsync(book);
                await libraryDbContext.SaveEntitiesAsync();
                return addedBook.Entity;
            }, IsValid);

            AddBook.Subscribe(b => BookAdded.Handle(Unit.Default).Subscribe());
        }
    }
}