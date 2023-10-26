using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ERNST.Helper.CustomValidation
{
    public class CheckEndDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var error = "";

                var startDateProperty = validationContext.ObjectType.GetProperty("StartDate");

                if (startDateProperty != null)
                {
                    var startDate = startDateProperty.GetValue(validationContext.ObjectInstance, null);

                    var startDateAsDateTime = DateTime.Parse(startDate.ToString());

                    var endDateAsDateTime = DateTime.Parse(value.ToString());


                    if (endDateAsDateTime <= startDateAsDateTime)
                    {
                         throw new Exception("يجب ان يكون تاريخ النهاية اكبر من تاريخ البدايه !");
                    }

                    var diff = endDateAsDateTime.Subtract(startDateAsDateTime).Days;

                    if (diff == 1)
                    {
                        throw new Exception("يجب ان يكون الفرق بين تاريخ النهاية والبدايه اكبر من يوم واحد !");
                    }
                }

                return ValidationResult.Success;
            }
            catch (Exception e)
            {
                return new ValidationResult(ErrorMessage ?? e.Message);
            }
        }
    }
}
