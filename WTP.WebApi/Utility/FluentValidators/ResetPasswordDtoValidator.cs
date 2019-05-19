using FluentValidation;
using WTP.BLL.Dto.AppUser;

namespace WTP.WebAPI.Utility.FluentValidators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .NotEmpty();
            RuleFor(dto => dto.NewPassword)
                .NotEmpty()
                .Length(4, 16);
            RuleFor(dto => dto.Code)
                .NotEmpty();
        }
    }
}
