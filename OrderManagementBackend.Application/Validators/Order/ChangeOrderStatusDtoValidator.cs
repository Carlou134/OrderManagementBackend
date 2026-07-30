using FluentValidation;
using OrderManagementBackend.Application.Dtos.Requests.Order;

namespace OrderManagementBackend.Application.Validators.Order
{
    public class ChangeOrderStatusDtoValidator : AbstractValidator<ChangeOrderStatusDto>
    {
        public ChangeOrderStatusDtoValidator()
        {
            RuleFor(x => x.status)
                .IsInEnum().WithMessage("Invalid status value.");
        }
    }
}
