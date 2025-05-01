using System;
using System.ComponentModel.DataAnnotations;
using FinanceTrackerAPI.Attributes;

namespace FinanceTrackerAPI.DTOs
{
    public record ExpenseDto
    {
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; init; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        public decimal Amount { get; init; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateNotInFuture(ErrorMessage = "Date cannot be in the future.")]
        public DateTime Date { get; init; }
    }

    public record ExpenseResponseDto(int Id, string Description, decimal Amount, DateTime Date);
}