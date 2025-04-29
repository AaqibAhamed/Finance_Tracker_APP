using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerAPI.Models;

//Expense Entity
public class Expense
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DateNotInFuture(ErrorMessage = "Date cannot be in the future.")]
    public DateTime Date { get; set; }
}