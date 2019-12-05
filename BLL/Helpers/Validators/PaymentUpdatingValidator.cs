using BLL.Models;
using FluentValidation;

namespace BLL.Helpers.Validators
{
    public class PaymentUpdatingValidator : AbstractValidator<PaymentModel>
    {
        public PaymentUpdatingValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Type).MaximumLength(10);
        }
    }
}