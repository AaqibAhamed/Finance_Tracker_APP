using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.Entities;
using FinanceTrackerModels.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomesController : ControllerBase
    {
        private readonly FinanceTrackerDbContext _context;

        public IncomesController(FinanceTrackerDbContext context)
        {
            _context = context;
        }

        // GET: api/Incomes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeResponseDto>>> GetIncomes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = "Date", [FromQuery] string? sortDirection = "desc")
        {
            var query = _context.Incomes.AsQueryable();

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "amount" => sortDirection?.ToLower() == "asc" ? query.OrderBy(i => i.Amount) : query.OrderByDescending(i => i.Amount),
                    "description" => sortDirection?.ToLower() == "asc" ? query.OrderBy(i => i.Description) : query.OrderByDescending(i => i.Description),
                    _ => sortDirection?.ToLower() == "asc" ? query.OrderBy(i => i.Date) : query.OrderByDescending(i => i.Date), // Default to sorting by date descending
                };
            }
            else
            {
                query = query.OrderByDescending(i => i.Date); // Default sorting
            }

            // Pagination
            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new IncomeResponseDto(i.Id, i.Description, i.Amount, i.Date)) // Project to DTO
                .ToListAsync();

            return Ok(new { Items = items, TotalCount = totalItems, PageNumber = pageNumber, PageSize = pageSize });
        }

        // GET: api/Incomes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeResponseDto>> GetIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            var responseDto = new IncomeResponseDto(income.Id, income.Description, income.Amount, income.Date);
            return responseDto;
        }

        // POST: api/Incomes
        [HttpPost]
        public async Task<ActionResult<IncomeResponseDto>> PostIncome(IncomeDto incomeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var income = new Income
            {
                Description = incomeDto.Description,
                Amount = incomeDto.Amount,
                Date = incomeDto.Date
            };

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();

            var responseDto = new IncomeResponseDto(income.Id, income.Description, income.Amount, income.Date);
            return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, responseDto);
        }

        // PUT: api/Incomes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncome(int id, IncomeDto incomeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            income.Description = incomeDto.Description;
            income.Amount = incomeDto.Amount;
            income.Date = incomeDto.Date;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncomeExists(id))
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

        // DELETE: api/Incomes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncomeExists(int id)
        {
            return _context.Incomes.Any(e => e.Id == id);
        }
    }
}