using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteka.Domain.Entities;
using Biblioteka.WPF.Events;
using Biblioteka.WPF.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Biblioteka.WPF.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen, ISupportsActivation
    {
        // The Router associated with this Screen.
        // Required by the IScreen interface.
        public RoutingState Router { get; }


        public MainViewModel()
        {
            Activator = new ViewModelActivator();
            Router = new RoutingState();
            this.WhenActivated(disposables =>
            {
                var login = new LoginViewModel();
                var userLogin = Observable.FromEventPattern<UserLoggedInEventArgs>(h => login.UserLoggedIn += h,
                    h => login.UserLoggedIn -= h).Take(1);
                Router.Navigate.Execute(login);

                userLogin.Subscribe(e => Router.Navigate.Execute(new BookListViewModel(this))).DisposeWith(disposables);
            });

        }

        public ViewModelActivator Activator { get; }
    }
}
