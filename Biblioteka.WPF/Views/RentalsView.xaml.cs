using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
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
using Biblioteka.WPF.ViewModels;
using Biblioteka.WPF.Views.ViewBases;
using ReactiveUI;

namespace Biblioteka.WPF.Views
{
    /// <summary>
    /// Interaction logic for RentalsView.xaml
    /// </summary>
    public partial class RentalsView : RentalsViewBase
    {
        public RentalsView()
        {
            InitializeComponent();
            ViewModel = new RentalsViewModel();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Rentals, v => v.BookList.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedRental, v => v.BookList.SelectedItem).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ReturnBookCommand, v => v.ReturnBookButton).DisposeWith(disposables);
            });
        }
    }
}
