using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerAPI.Attributes;

public class DateNotInFutureAttribute : ValidationAttribute
{
    public DateNotInFutureAttribute()
    {
        ErrorMessage = "The date cannot be in the future."; // Default error message
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true; // Allow null values (optional fields)
        }

        if (value is DateTime date)
        {
            return date <= DateTime.Now.Date;
        }

        return false; // Invalid type
    }
}