using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Biblioteka.Domain.Entities;
using Biblioteka.Persistence;
using DynamicData;
using DynamicData.Binding;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Biblioteka.WPF.ViewModels
{
    public class BookListViewModel : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        public string UrlPathSegment => "bookList";

        public IScreen HostScreen { get; }

        public ReadOnlyObservableCollection<Book> BookList { get; private set; }

        [Reactive]
        public Book SelectedBook { get; set; }

        [Reactive]
        public string SearchTermTitle { get; set; }

        [Reactive]
        public string SearchTermAuthor { get; set; }

        [Reactive]
        public int? SearchTermYear { get; set; }

        [Reactive]
        public string SearchTermPublisher { get; set; }

        [Reactive]
        public bool SearchAvailable { get; set; }

        public ReactiveCommand<Unit, Unit> LoadBooksCommand { get; }


        public ReactiveCommand<Unit, Unit> RemoveBookCommand { get; }

        public ReactiveCommand<Unit, BookRental> RentBookCommand { get; }

        [Reactive]
        public User User { get; set; }

        public BookListViewModel(IScreen hostScreen = null, LibraryDbContext dbContext = null, IUserService userService = null)
        {
            Activator = new ViewModelActivator();
            userService = userService ?? Locator.Current.GetService<IUserService>();
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
            dbContext = dbContext ?? Locator.Current.GetService<LibraryDbContext>();
            LoadBooksCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            {
                await dbContext.Books.Include(b => b.Rental).LoadAsync();
                return Unit.Default;
            });

            var isBookSelected = this.WhenAnyValue(vm => vm.SelectedBook).Select(b => b != null);
            var isBookAvailable = this.WhenAnyValue(vm => vm.SelectedBook).Select(b => b?.IsAvailable ?? false);

            RemoveBookCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            {
                dbContext.Books.Remove(SelectedBook);
                await dbContext.SaveEntitiesAsync();
                return Unit.Default;
            }, isBookSelected);

            RentBookCommand = ReactiveCommand.CreateFromTask<Unit, BookRental>(async _ =>
            {
                var rental = SelectedBook.RentBook(userService.CurrentUser);
                await dbContext.SaveEntitiesAsync();
                return rental;
            }, isBookAvailable);

            this.WhenActivated(disposables => { LoadBooksCommand.Execute().Subscribe().DisposeWith(disposables); });

            var filterTitle = this.WhenAnyValue(vm => vm.SearchTermTitle)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .DistinctUntilChanged()
                .Select(BuildFilterTitle);

            var filterAuthor = this.WhenAnyValue(vm => vm.SearchTermAuthor)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .DistinctUntilChanged()
                .Select(BuildFilterAuthor);

            var filterPublisher = this.WhenAnyValue(vm => vm.SearchTermPublisher)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .DistinctUntilChanged()
                .Select(BuildFilterPublisher);

            var filterYearPublished = this.WhenAnyValue(vm => vm.SearchTermYear)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .DistinctUntilChanged()
                .Select(BuildFilterYearPublished);

            var filterAvailable = this.WhenAnyValue(vm => vm.SearchAvailable)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .DistinctUntilChanged()
                .Select(BuildFilterAvailable);

            var bookList = dbContext.Books.Local.ToObservableCollection().ToObservableChangeSet(b => b.Id);
            bookList.AutoRefreshOnObservable(_ => RentBookCommand)
                .AutoRefreshOnObservable(_ => LoadBooksCommand)
                .AutoRefreshOnObservable(_ => dbContext.BookRentals.Local.ToObservableCollection().ToObservableChangeSet())
                .Filter(filterTitle)
                .Filter(filterAuthor)
                .Filter(filterPublisher)
                .Filter(filterYearPublished)
                .Filter(filterAvailable)
                .Filter(b => b.Rental == null)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out var data)
                .DisposeMany()
                .Subscribe();
            BookList = data;
        }

        public ViewModelActivator Activator { get; }

        private Func<Book, bool> BuildFilterTitle(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return b => true;

            return b =>
                CultureInfo.CurrentCulture.CompareInfo.IndexOf(b.Title, searchText, CompareOptions.IgnoreCase) >= 0;
        }

        private Func<Book, bool> BuildFilterAuthor(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return b => true;

            return b =>
                CultureInfo.CurrentCulture.CompareInfo.IndexOf(b.Author, searchText, CompareOptions.IgnoreCase) >= 0;
        }

        private Func<Book, bool> BuildFilterPublisher(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return b => true;

            return b => b.Publisher == searchText;
        }

        private Func<Book, bool> BuildFilterYearPublished(int? yearPublished)
        {
            if (yearPublished == default)
                return b => true;

            return b => b.YearPublished == yearPublished;
        }

        private Func<Book, bool> BuildFilterAvailable(bool availableOnly)
        {
            if (availableOnly == false)
                return b => true;

            return b => b.IsAvailable;
        }
    }
}
