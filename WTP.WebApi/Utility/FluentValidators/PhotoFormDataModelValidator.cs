using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using WTP.BLL.DTOs.ServicesDTOs;

namespace WTP.WebAPI.Utility.FluentValidators
{
    public class PhotoFormDataModelValidator : AbstractValidator<PhotoFormDataDto>
    {
        private readonly string[] _supportedTypes = new[] { "png", "jpg", "jpeg" };

        private readonly string _errorMessage = "Photo upload faild. File extention must be: png, jpg.";

        public PhotoFormDataModelValidator()
        {
            RuleFor(form => form.File)
                    .Must(IsImage)
                    .WithMessage(_errorMessage);
        }

        private bool IsImage(IFormFile file)
        {
            var fileExt = Path.GetExtension(file.FileName).Substring(1).ToLower();

            return _supportedTypes.Contains(fileExt);
        }
    }
}
