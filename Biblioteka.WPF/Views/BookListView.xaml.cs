using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Biblioteka.WPF.Models;
using Biblioteka.WPF.ViewModels;
using Biblioteka.WPF.Views.ViewBases;
using ReactiveUI;
using Splat;

namespace Biblioteka.WPF.Views
{
    /// <summary>
    /// Interaction logic for BookListView.xaml
    /// </summary>
    public partial class BookListView : BookListViewBase
    {
        public BookListView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.BookList, v => v.BookList.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedBook, v => v.BookList.SelectedItem).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.RemoveBookCommand, v => v.DeleteButton).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SearchTermTitle, v => v.SearchTermTitle.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SearchTermAuthor, v => v.SearchTermAuthor.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SearchTermYear, v => v.SearchTermYear.Text, i => i.ToString(), s => int.TryParse(s, out var tempVal) ? tempVal : (int?)null).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SearchTermPublisher, v => v.SearchTermPublisher.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SearchAvailable, v => v.SearchAvailable.IsChecked).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.RentBookCommand, v => v.RentButton).DisposeWith(disposables);
            });
            

        }
    }
}
