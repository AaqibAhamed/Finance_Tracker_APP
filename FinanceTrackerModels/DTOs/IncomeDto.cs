using System.ComponentModel.DataAnnotations;
using FinanceTrackerModels.Attributes;

namespace FinanceTrackerModels.DTOs
{
    public record IncomeDto
    {
        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(200)]
        public required string Description { get; init; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        public decimal Amount { get; init; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateNotInFuture(ErrorMessage = "The date cannot be in the future.")]
        public required DateTime Date { get; init; }
    }

    public record IncomeResponseDto(int Id, string Description, decimal Amount, DateTime Date);
}