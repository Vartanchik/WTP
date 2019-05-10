using FluentValidation;
using WTP.WebAPI.ViewModels;

namespace WTP.WebAPI.FluentValidators
{
    public class ChangePassworViewModeldValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePassworViewModeldValidator()
        {
            RuleFor(model => model.NewPassword)
                .NotEqual(model => model.CurrentPassword)
                .Length(4, 16);
        }
    }
}
