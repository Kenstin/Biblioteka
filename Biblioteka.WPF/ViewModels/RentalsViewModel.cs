using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
    public class RentalsViewModel : ReactiveObject, ISupportsActivation
    {
        public ReadOnlyObservableCollection<BookRental> Rentals { get; }

        [Reactive]
        public BookRental SelectedRental { get; set; }

        public ReactiveCommand<Unit, Unit> LoadRentalsCommand { get; }

        public ReactiveCommand<Unit, Unit> ReturnBookCommand { get; }

        public RentalsViewModel(LibraryDbContext dbContext = null, IUserService userService = null)
        {
            Activator = new ViewModelActivator();
            userService = userService ?? Locator.Current.GetService<IUserService>();
            dbContext = dbContext ?? Locator.Current.GetService<LibraryDbContext>();

            LoadRentalsCommand = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
            {
                await dbContext.BookRentals.Include(r => r.Book).Include(r => r.User).LoadAsync();
                return Unit.Default;
            });

            var isRentalSelected = this.WhenAnyValue(vm => vm.SelectedRental).Select(b => b != null);

            ReturnBookCommand =
                ReactiveCommand.CreateFromTask<Unit, Unit>(async _ =>
                    {
                        SelectedRental.Book.ReturnBook();
                        await dbContext.SaveEntitiesAsync();
                        return Unit.Default;
                    },
                    isRentalSelected);


            this.WhenActivated(disposables =>
            {
                LoadRentalsCommand.Execute().Subscribe().DisposeWith(disposables);
            });

            var bookList = dbContext.BookRentals.Local.ToObservableCollection().ToObservableChangeSet(b => b.Id);
            bookList.AutoRefreshOnObservable(_ => LoadRentalsCommand)
                .Filter(r => r.User == userService.CurrentUser)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out var data)
                .Subscribe();
            Rentals = data;
        }

        public ViewModelActivator Activator { get; }
    }
}
