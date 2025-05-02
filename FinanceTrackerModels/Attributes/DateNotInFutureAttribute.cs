using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerModels.Attributes;

public class DateNotInFutureAttribute : ValidationAttribute
{
    public DateNotInFutureAttribute()
    {
        ErrorMessage = "The date cannot be in the future."; // Default error message
    }

    public override bool IsValid(object? value)
    {
        return value switch
        {
            null => true,
            DateTime date => date <= DateTime.Now.Date,
            _ => false
        };
    }
}