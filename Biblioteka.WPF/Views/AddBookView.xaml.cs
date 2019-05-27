using System.Reactive;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using Biblioteka.WPF.ViewModels;
using Biblioteka.WPF.Views.ViewBases;
using ReactiveUI;

namespace Biblioteka.WPF.Views
{
    /// <summary>
    /// Interaction logic for AddBookView.xaml
    /// </summary>
    public partial class AddBookView : AddBookViewBase, IViewFor<AddBookViewModel>
    {
        public AddBookView()
        {
            InitializeComponent();
            ViewModel = new AddBookViewModel();

            this.WhenActivated(disposableRegistration =>
            {
                this.Bind(ViewModel, vm => vm.Author, v => v.Author.Text)
                    .DisposeWith(disposableRegistration);
                this.Bind(ViewModel, vm => vm.Title, v => v.Title.Text)
                    .DisposeWith(disposableRegistration);
                this.Bind(ViewModel, vm => vm.Publisher, v => v.Publisher.Text)
                    .DisposeWith(disposableRegistration);
                this.Bind(ViewModel, vm => vm.YearPublished, v => v.YearPublished.Text, i => i.ToString(), s => int.TryParse(s, out var tempVal) ? tempVal : (int?)null)
                    .DisposeWith(disposableRegistration);
                this.Bind(ViewModel, vm => vm.Available, v => v.Available.IsChecked)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel, vm => vm.Errors, v => v.Errors.Text)
                    .DisposeWith(disposableRegistration);
                this.BindCommand(ViewModel, vm => vm.AddBook, v => v.SaveBook)
                    .DisposeWith(disposableRegistration);
                
                ViewModel.BookAdded.RegisterHandler(context =>
                {
                    MessageBox.Show("Ksiazka zostala dodana");
                    context.SetOutput(Unit.Default);
                });
            });
        }
    }
}
