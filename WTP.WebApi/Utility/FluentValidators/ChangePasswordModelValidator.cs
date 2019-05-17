using FluentValidation;
using WTP.WebAPI.Models;

namespace WTP.WebAPI.FluentValidators
{
    public class ChangePassworModelValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePassworModelValidator()
        {
            RuleFor(model => model.NewPassword)
                .NotEqual(model => model.CurrentPassword)
                .Length(4, 16);
        }
    }
}
