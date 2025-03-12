using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Features.PharmacyInfo.Validators
{
    public class PharmacyLogoValidator : AbstractValidator<IFormFile>
    {
        public PharmacyLogoValidator() 
        {
            RuleFor(f => f.ContentType)
                .Must(c => c.Equals("image/jpeg") || c.Equals("image/jpg") || c.Equals("image/png"))
                .WithMessage("{PropertyName} can not be other than jpeg, jpg and png");

            RuleFor(f => f.Length)
                .LessThanOrEqualTo(2000000)
                .WithMessage("{PropertyName} should be less than 2MB");
        }
        
    }
}
