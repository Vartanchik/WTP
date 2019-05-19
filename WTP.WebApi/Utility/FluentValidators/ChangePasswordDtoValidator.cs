using FluentValidation;
using WTP.BLL.Dto.AppUser;
using WTP.WebAPI.Models;

namespace WTP.WebAPI.FluentValidators
{
    public class ChangePassworDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePassworDtoValidator()
        {
            RuleFor(model => model.CurrentPassword)
                .NotEmpty()
                .Length(4, 16);
            RuleFor(model => model.NewPassword)
                .NotEmpty()
                .NotEqual(model => model.CurrentPassword)
                .Length(4, 16);
        }
    }
}
