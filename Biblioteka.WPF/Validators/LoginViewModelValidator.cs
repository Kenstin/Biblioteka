using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteka.WPF.ViewModels;
using FluentValidation;

namespace Biblioteka.WPF.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(vm => vm.Login).NotEmpty().MaximumLength(64);
            RuleFor(vm => vm.Password).NotEmpty().MinimumLength(4).MaximumLength(64);
        }
    }
}
