using Biblioteka.WPF.ViewModels;
using FluentValidation;

namespace Biblioteka.WPF.Validators
{
    public class AddBookViewModelValidator : AbstractValidator<AddBookViewModel>
    {
        public AddBookViewModelValidator()
        {
            RuleFor(vm => vm.Author).NotEmpty().MaximumLength(64).WithMessage("Niepoprawny autor.");
            RuleFor(vm => vm.Publisher).NotEmpty().MaximumLength(64).WithMessage("Niepoprawny wydawca.");
            RuleFor(vm => vm.Title).NotEmpty().MaximumLength(64).WithMessage("Niepoprawny tytul.");
            RuleFor(vm => vm.YearPublished).NotEmpty().GreaterThan(0).WithMessage("Rok wydania musi byc wiekszy od 0.");
        }
    }
}
