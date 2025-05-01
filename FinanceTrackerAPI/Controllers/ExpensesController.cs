using FinanceTrackerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerAPI.DTOs;
using FinanceTrackerAPI.Entities;

namespace FinanceTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly FinanceTrackerDbContext _context;

        public ExpensesController(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseResponseDto>>> GetExpenses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = "Date", [FromQuery] string? sortDirection = "desc")
        {
            var query = _context.Expenses.AsQueryable();

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "amount" => sortDirection?.ToLower() == "asc" ? query.OrderBy(e => e.Amount) : query.OrderByDescending(e => e.Amount),
                    "description" => sortDirection?.ToLower() == "asc" ? query.OrderBy(e => e.Description) : query.OrderByDescending(e => e.Description),
                    _ => sortDirection?.ToLower() == "asc" ? query.OrderBy(e => e.Date) : query.OrderByDescending(e => e.Date), // Default to sorting by date descending
                };
            }
            else
            {
                query = query.OrderByDescending(e => e.Date); // Default sorting
            }

            // Pagination
            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ExpenseResponseDto(e.Id, e.Description, e.Amount, e.Date)) // Project to DTO
                .ToListAsync();

            return Ok(new { Items = items, TotalCount = totalItems, PageNumber = pageNumber, PageSize = pageSize });
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseResponseDto>> GetExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            var responseDto = new ExpenseResponseDto(expense.Id, expense.Description, expense.Amount, expense.Date);
            return responseDto;
        }

        // POST: api/Expenses
        [HttpPost]
        public async Task<ActionResult<ExpenseResponseDto>> PostExpense(ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var expense = new Expense
            {
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                Date = expenseDto.Date
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var responseDto = new ExpenseResponseDto(expense.Id, expense.Description, expense.Amount, expense.Date);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, responseDto);
        }

        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            expense.Description = expenseDto.Description;
            expense.Amount = expenseDto.Amount;
            expense.Date = expenseDto.Date;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}