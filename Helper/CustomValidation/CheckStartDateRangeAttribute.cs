using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Helper.CustomValidation
{
    public class CheckStartDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var dt = DateTime.Parse(value.ToString());
                return dt > DateTime.UtcNow ? ValidationResult.Success : throw new Exception("يجب ان يكون تاريخ البدايه اكبر من تاريخ اليوم !");
            }
            catch (Exception e)
            {
                return new  ValidationResult(ErrorMessage ?? e.Message);
            }
        }
    }
}
