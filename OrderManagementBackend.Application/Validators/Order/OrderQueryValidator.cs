using FluentValidation;
using OrderManagementBackend.Application.Dtos.Requests.Order;

namespace OrderManagementBackend.Application.Validators.Order
{
    public class OrderQueryValidator : AbstractValidator<OrderQuery>
    {
        public OrderQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than zero.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value.")
                .When(x => x.Status.HasValue);
        }
    }
}
