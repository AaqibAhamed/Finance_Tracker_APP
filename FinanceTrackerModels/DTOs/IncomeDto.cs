using System.ComponentModel.DataAnnotations;
using FinanceTrackerModels.Attributes;

namespace FinanceTrackerModels.DTOs
{
    public record IncomeDto
    {
        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(200)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount must be a positive number.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateNotInFuture(ErrorMessage = "The date cannot be in the future.")]
        public DateTime? Date { get; set; }
    }

    public record IncomeResponseDto(int Id, string Description, decimal Amount, DateTime? Date);
}