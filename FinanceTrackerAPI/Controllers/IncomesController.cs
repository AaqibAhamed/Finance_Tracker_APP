using FinanceTrackerAPI.Data;
using FinanceTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerAPI.Controllers;

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
        public async Task<ActionResult<IEnumerable<Income>>> GetIncomes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortBy = "Date", [FromQuery] string? sortDirection = "desc")
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
                .ToListAsync();

            return Ok(new { Items = items, TotalCount = totalItems, PageNumber = pageNumber, PageSize = pageSize });
        }

        // GET: api/Incomes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Income>> GetIncome(int id)
        {
            var income = await _context.Incomes.FindAsync(id);

            if (income == null)
            {
                return NotFound();
            }

            return income;
        }

        // POST: api/Incomes
        [HttpPost]
        public async Task<ActionResult<Income>> PostIncome(Income income)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncome), new { id = income.Id }, income);
        }

        // PUT: api/Incomes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncome(int id, Income income)
        {
            if (id != income.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(income).State = EntityState.Modified;

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