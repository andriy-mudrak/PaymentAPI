using BLL.Models;
using FluentValidation;

namespace BLL.Helpers.Validators
{
    public class PaymentCreatingValidator : AbstractValidator<PaymentModel>
    {
        public PaymentCreatingValidator()
        {
            RuleFor(x => x.Amount).NotEmpty();
            RuleFor(x => x.CardToken).MaximumLength(50);
            RuleFor(x => x.Currency).MaximumLength(5);
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.VendorId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.SaveCard).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Type).MaximumLength(10);
        }
    }
}