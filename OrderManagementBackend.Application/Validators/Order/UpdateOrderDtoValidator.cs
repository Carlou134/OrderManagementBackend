using FluentValidation;
using OrderManagementBackend.Application.Dtos.Requests.Order;
using OrderManagementBackend.Application.Validators.OrderProduct;

namespace OrderManagementBackend.Application.Validators.Order
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("OrderNumber is required.")
                .MaximumLength(10).WithMessage("OrderNumber must not exceed 10 characters.");

            RuleFor(x => x.Products)
                .NotEmpty().WithMessage("An order must have at least one product.");

            RuleForEach(x => x.Products).SetValidator(new UpdateOrderProductDtoValidator());
        }
    }
}
