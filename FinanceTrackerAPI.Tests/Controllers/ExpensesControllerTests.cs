using FinanceTrackerAPI.Controllers;
using FinanceTrackerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerAPI.Entities;
using FinanceTrackerModels.DTOs;

namespace FinanceTrackerAPI.Tests.Controllers
{
    public class ExpensesControllerTests
    {
        private FinanceTrackerDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FinanceTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new FinanceTrackerDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetExpenses_ReturnsOkResultWithExpenses()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            context.Expenses.AddRange(new Expense { Description = "Rent", Amount = 1000, Date = DateTime.Now.AddDays(-2) },
                                      new Expense { Description = "Groceries", Amount = 200, Date = DateTime.Now });
            await context.SaveChangesAsync();
            var controller = new ExpensesController(context);

            // Act
            var result = await controller.GetExpenses();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue =  okResult.Value;
            var itemsProperty = returnValue?.GetType().GetProperty("Items");
            var expenses = Assert.IsAssignableFrom<IEnumerable<ExpenseResponseDto>>(itemsProperty?.GetValue(returnValue));

            // Assert
            Assert.Equal(2, expenses.Count());
        }

        [Fact]
        public async Task GetExpense_WithValidId_ReturnsOkResultWithExpense()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var expense = new Expense { Id = 1, Description = "Utilities", Amount = 150, Date = DateTime.Now };
            context.Expenses.Add(expense);
            await context.SaveChangesAsync();
            var controller = new ExpensesController(context);

            // Act
            var result = await controller.GetExpense(1);
            var okResult = Assert.IsType<ActionResult<ExpenseResponseDto>>(result);
            Assert.IsType<ExpenseResponseDto>(okResult.Value);
            Assert.Equal("Utilities", okResult.Value?.Description);
        }

        [Fact]
        public async Task GetExpense_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var controller = new ExpensesController(context);

            // Act
            var result = await controller.GetExpense(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostExpense_WithValidModel_ReturnsCreatedAtActionResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var expenseDto = new ExpenseDto { Description = "Dinner", Amount = 30, Date = DateTime.Now };
            var controller = new ExpensesController(context);

            // Act
            var result = await controller.PostExpense(expenseDto);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdExpense = Assert.IsType<ExpenseResponseDto>(createdAtActionResult.Value);

            // Assert
            Assert.Equal(expenseDto.Description, createdExpense.Description);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task PostExpense_WithInvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var expenseDto = new ExpenseDto {Description = "Expense Desc",Date = DateTime.Now, Amount = -50 }; // Invalid amount
            var controller = new ExpensesController(context);
            controller.ModelState.AddModelError("Amount", "Amount must be positive.");

            // Act
            var result = await controller.PostExpense(expenseDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutExpense_WithValidIdAndModel_ReturnsNoContentResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var initialExpense = new Expense { Id = 1, Description = "Old Bills", Amount = 250, Date = DateTime.Now.AddDays(-3) };
            context.Expenses.Add(initialExpense);
            await context.SaveChangesAsync();
            var updatedExpenseDto = new ExpenseDto { Description = "New Bills", Amount = 300, Date = DateTime.Now };
            var controller = new ExpensesController(context);

            // Act
            var result = await controller.PutExpense(1, updatedExpenseDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutExpense_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var expenseDto = new ExpenseDto { Description = "Lunch", Amount = 20, Date = DateTime.Now };
            var controller = new ExpensesController(context);

            // Act
            var result = await controller.PutExpense(99, expenseDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutExpense_WithInvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var expenseDto = new ExpenseDto {Description = "Outing",Date = DateTime.Now, Amount = -10 }; // Invalid amount
            var controller = new ExpensesController(context);
            controller.ModelState.AddModelError("Amount", "Amount must be positive.");

            // Act
            var result = await controller.PutExpense(1, expenseDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteExpense_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var expense = new Expense { Id = 1, Description = "Coffee", Amount = 3, Date = DateTime.Now.AddDays(-1) };
            context.Expenses.Add(expense);
            await context.SaveChangesAsync();
            var controller = new ExpensesController(context);

            // Act
            var result = await controller.DeleteExpense(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteExpense_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var controller = new ExpensesController(context);

            // Act
            var result = await controller.DeleteExpense(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}