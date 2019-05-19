using FluentValidation;
using System;
using System.Globalization;
using WTP.BLL.Dto.AppUser;

namespace WTP.WebAPI.Utility.FluentValidators
{
    public class AppUserDtoValidator : AbstractValidator<AppUserDto>
    {
        public AppUserDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .NotEmpty();
            RuleFor(dto => dto.UserName)
                .NotEmpty()
                .Length(4, 30);
            RuleFor(dto => dto.Email)
                .EmailAddress();
            RuleFor(dto => dto.Gender)
                .NotEmpty();
            RuleFor(dto => dto.DateOfBirth)
                .NotEmpty()
                .Must(IsValidDate);
            RuleFor(dto => dto.Country)
                .NotEmpty();
            RuleFor(dto => dto.Languages)
                .NotEmpty();
        }

        private bool IsValidDate(DateTime? value)
        {
            string[] formats = new string[] { "yyyy-MM-dd", "dd-MM-yyyy", "dd.MM.yyyy", "yyyy.MM.dd" };
            var result = DateTime.TryParseExact(value.ToString(),
                                                formats,
                                                CultureInfo.InvariantCulture,
                                                DateTimeStyles.NoCurrentDateDefault,
                                                result: out _);
            return result;
        }
    }
}
