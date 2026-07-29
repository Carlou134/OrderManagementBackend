using FluentValidation;
using OrderManagementBackend.Application.Dtos.Requests.OrderProduct;

namespace OrderManagementBackend.Application.Validators.OrderProduct
{
    public class UpdateOrderProductDtoValidator : AbstractValidator<UpdateOrderProductDto>
    {
        public UpdateOrderProductDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be a valid product identifier.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
