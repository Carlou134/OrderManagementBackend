using FluentValidation;
using OrderManagementBackend.Application.Dtos.Requests.Product;

namespace OrderManagementBackend.Application.Validators.Product
{
    public class ProductQueryValidator : AbstractValidator<ProductQuery>
    {
        public ProductQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than zero.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
        }
    }
}
