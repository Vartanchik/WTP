using FluentValidation;
using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using WTP.WebAPI.Dto;

namespace WTP.WebAPI.FluentValidators
{
    public class AppUserDtoValidator : AbstractValidator<AppUserApiDto>
    {
        public AppUserDtoValidator()
        {
            RuleFor(model => model.UserName)
                .Length(4, 16);

            RuleFor(model => model.DateOfBirth)
                .Must(IsValidDate);

            //RuleFor(model => model.Photo)
            //    .Must(IsUrlToPicture);

            //RuleFor(model => model.Steam)
            //    .Must(IsValideSteamAccUrl);
        }

        private bool IsValidDate(string value)
        {
            string[] formats = new string[] { "yyyy-MM-dd", "dd-MM-yyyy", "dd.MM.yyyy", "yyyy.MM.dd" };
            var result = DateTime.TryParseExact(value,
                                                formats,
                                                CultureInfo.InvariantCulture,
                                                DateTimeStyles.NoCurrentDateDefault,
                                                result: out _);
            return result;
        }

        /*
        private bool IsUrlToPicture(string value)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(value);
            request.Method = "HEAD";
            using (var response = request.GetResponse())
            {
                return response.ContentType.ToLower(CultureInfo.InvariantCulture)
                           .StartsWith("image/");
            }
        }

        private bool IsValideSteamAccUrl(string value)
        {
            var regex = "(?:https?:\\/\\/)?steamcommunity\\.com\\/(?:profiles|id)\\/[a-zA-Z0-9]+";
            var match = Regex.Match(value, regex, RegexOptions.IgnoreCase);

            return match.Success;
        }
        */
    }
}
