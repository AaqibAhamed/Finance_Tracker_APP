using FinanceTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerAPI.Data;

public class FinanceTrackerDbContext : DbContext
{
    public FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) : base(options)
    {
    }

    public DbSet<Income> Incomes { get; set; }
    public DbSet<Expense> Expenses { get; set; }
}