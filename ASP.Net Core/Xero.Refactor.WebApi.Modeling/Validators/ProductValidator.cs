using FluentValidation;

namespace Xero.Refactor.WebApi.Modeling.Validators
{
    public class ProductValidator : AbstractValidator<ProductApiModel>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Description).MaximumLength(500);
            RuleFor(p => p.Price).GreaterThanOrEqualTo(0);
            RuleFor(p => p.DeliveryPrice).GreaterThanOrEqualTo(0);
        }
    }
}