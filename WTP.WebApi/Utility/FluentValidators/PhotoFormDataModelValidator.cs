using FluentValidation;
using System;
using System.Linq;
using WTP.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WTP.WebAPI.Utility.FluentValidators
{
    public class PhotoFormDataModelValidator : AbstractValidator<PhotoFormDataModel>
    {
        private readonly string[] _supportedTypes = new[] { "png", "jpg" };

        private readonly string _errorMessage = "Photo upload faild. File extention must be: png, jpg.";

        public PhotoFormDataModelValidator()
        {
            RuleFor(form => form.File)
                    .Must(IsImage)
                    .WithMessage(_errorMessage);
        }

        private bool IsImage(IFormFile file)
        {

            var fileExt = Path.GetExtension(file.FileName).Substring(1);

            if (_supportedTypes.Contains(fileExt))
            {
                return true;
            }

            return false;
        }
    }
}
