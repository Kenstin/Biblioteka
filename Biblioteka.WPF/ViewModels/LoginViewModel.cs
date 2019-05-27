using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using Biblioteka.Domain.Entities;
using Biblioteka.Persistence;
using Biblioteka.WPF.Events;
using Biblioteka.WPF.Models;
using Biblioteka.WPF.Validators;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Biblioteka.WPF.ViewModels
{
    public class LoginViewModel : ValidatedReactiveObject<LoginViewModel, LoginViewModelValidator>, IRoutableViewModel
    {
        public string UrlPathSegment => "login";
        public IScreen HostScreen { get; }

        [Reactive]
        public string Login { get; set; }

        public string LoginErrors { [ObservableAsProperty] get; }

        [Reactive]
        public string Password { get; set; }

        public string PasswordErrors { [ObservableAsProperty] get; }


        public event EventHandler<UserLoggedInEventArgs> UserLoggedIn;

        public LoginInfo LoginInfo { [ObservableAsProperty] get; }

        public ReactiveCommand<LoginInfo, User> LoginCommand { get; }

        public Interaction<Unit, Unit> LoginFailed { get; }

        protected virtual void OnUserLoggedIn(UserLoggedInEventArgs e)
        {
            var handler = UserLoggedIn;
            handler?.Invoke(this, e);
        }

        public LoginViewModel(IScreen screen = null, IUserService userService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
                userService = userService?? Locator.Current.GetService<IUserService>();
            LoginFailed = new Interaction<Unit, Unit>();
            Setup(vm => vm.Login, vm => vm.LoginErrors);
            Setup(vm => vm.Password, vm => vm.PasswordErrors);

            this.WhenAnyValue(vm => vm.Login, vm => vm.Password)
                .Where(tuple => tuple.Item1 != null && tuple.Item2 != null)
                .Select((tuple => new LoginInfo(tuple.Item1, tuple.Item2))).ToPropertyEx(this, vm => vm.LoginInfo);

            LoginCommand = ReactiveCommand.CreateFromTask<LoginInfo, User>(async loginInfo =>
            {
                var user = await userService.TryLogin(loginInfo.Username, loginInfo.Password);
                if (user != null)
                {
                    OnUserLoggedIn(new UserLoggedInEventArgs() {User = user});
                    return user;
                }
                else
                {
                    throw new Exception(); // to powinno miec swoje wlasne exception XD
                }
            }, IsValid);

            LoginCommand.ThrownExceptions.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(e => LoginFailed.Handle(Unit.Default).Subscribe());
        }
    }
}