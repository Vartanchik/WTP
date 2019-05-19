using FluentValidation;
using WTP.BLL.DTOs.ServicesDTOs;

namespace WTP.WebAPI.FluentValidators
{
    public class ChangePassworDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePassworDtoValidator()
        {
            RuleFor(model => model.NewPassword)
                .NotEqual(model => model.CurrentPassword)
                .Length(4, 16);
        }
    }
}
