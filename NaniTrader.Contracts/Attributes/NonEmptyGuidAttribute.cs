using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaniTrader.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class NonEmptyGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is Guid guid && Guid.Empty == guid)
            {
                return new ValidationResult($"{validationContext.DisplayName} cannot be empty.");
            }
            return ValidationResult.Success;
        }
    }
}
