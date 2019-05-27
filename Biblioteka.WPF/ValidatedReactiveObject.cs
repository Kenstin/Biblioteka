using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using FluentValidation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Biblioteka.WPF
{
    public class ValidatedReactiveObject<T, TV> : ReactiveObject
    where T : ValidatedReactiveObject<T, TV>
    where TV : class, IValidator, new()
    {
        protected TV Validator;
        public IObservable<bool> IsValid { get; private set; }

        public ValidatedReactiveObject()
        {
            Validator = new TV();
            if (!Validator.CanValidateInstancesOfType(GetType()))
            {
                throw new InvalidOperationException($"Provided validator does not support this type: {GetType()}");
            }

            IsValid = Observable.Never<bool>();
        }

        protected void Setup<TProperty> (Expression<Func<T, TProperty>> property, Expression<Func<T, string>> errorProperty)
        {
            var results = ((T) this).WhenAnyValue(property)
                .Select(x => Validator.Validate(this));
            IsValid = IsValid.Merge(results.Select(result => result.IsValid));

            var xd = results.SelectMany(validationResult => validationResult.Errors
                    .Where(failure => failure.PropertyName == property.GetMemberName())
                    .Select(failure => failure.ErrorMessage).DefaultIfEmpty(null))
                .ToPropertyEx((T) this, errorProperty);
        }

    }
}
