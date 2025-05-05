namespace FinanceTrackerModels.DTOs;

public class GetExpenseDto
{
    public int Id { get; init; }
    
    public string? Description { get; set; }
    
    public decimal Amount { get; set; }
    
    public  DateTime? Date { get; set; }
    
}