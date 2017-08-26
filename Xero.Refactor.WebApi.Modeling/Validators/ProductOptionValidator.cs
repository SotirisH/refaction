using FluentValidation;

namespace Xero.Refactor.WebApi.Modeling.Validators
{
    public class ProductOptionValidator : AbstractValidator<ProductOptionApiModel>
    {
        public ProductOptionValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Description).MaximumLength(500);
            RuleFor(p => p.RowVersion).NotEmpty();
            RuleFor(p => p.ProductId).NotEmpty();
        }
    }
}
