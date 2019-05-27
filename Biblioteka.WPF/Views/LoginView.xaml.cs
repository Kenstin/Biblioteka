using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Biblioteka.WPF.Models;
using Biblioteka.WPF.ViewModels;
using Biblioteka.WPF.Views.ViewBases;
using ReactiveUI;

namespace Biblioteka.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : LoginViewBase  
    {
        public LoginView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
           {
               this.Bind(ViewModel, vm => vm.Login, v => v.Username.Text).DisposeWith(disposables);
               Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                   handler => Password.PasswordChanged += handler,
                   handler => Password.PasswordChanged -= handler).Subscribe(args => ViewModel.Password = Password.Password).DisposeWith(disposables);


               this.BindCommand(ViewModel, vm => vm.LoginCommand, v => v.Login, vm => vm.LoginInfo).DisposeWith(disposables);

               ViewModel.LoginFailed
                   .RegisterHandler(context =>
                   {
                       System.Windows.MessageBox.Show("Niepoprawne dane logowania");
                       context.SetOutput(Unit.Default);
                   })
                   .DisposeWith(disposables);
           });

        }
    }
}
