using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerAPI.Entities;

public class Income
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public string Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}
