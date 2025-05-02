using FinanceTrackerAPI.Controllers;
using FinanceTrackerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerAPI.Entities;
using FinanceTrackerModels.DTOs;

namespace FinanceTrackerAPI.Tests.Controllers;

public class IncomesControllerTests
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
        public async Task GetIncomes_ReturnsOkResultWithIncomes()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            context.Incomes.AddRange(new Income { Description = "Salary", Amount = 2000, Date = DateTime.Now.AddDays(-1) },
                new Income { Description = "Bonus", Amount = 500, Date = DateTime.Now });
            await context.SaveChangesAsync();
            var controller = new IncomesController(context);

            // Act
            var result = await controller.GetIncomes();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = okResult.Value;
            var itemsProperty = returnValue?.GetType().GetProperty("Items");
            var incomes = Assert.IsAssignableFrom<IEnumerable<IncomeResponseDto>>(itemsProperty?.GetValue(returnValue));

            // Assert
            Assert.Equal(2, incomes.Count());
        }

        [Fact]
        public async Task GetIncome_WithValidId_ReturnsOkResultWithIncome()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var income = new Income { Id = 1, Description = "Salary", Amount = 2000, Date = DateTime.Now };
            context.Incomes.Add(income);
            await context.SaveChangesAsync();
            var controller = new IncomesController(context);

            // Act
            var result = await controller.GetIncome(1);
            var okResult = Assert.IsType<ActionResult<IncomeResponseDto>>(result);
            Assert.IsType<IncomeResponseDto>(okResult.Value);
            Assert.Equal("Salary", okResult.Value?.Description);
        }

        [Fact]
        public async Task GetIncome_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var controller = new IncomesController(context);

            // Act
            var result = await controller.GetIncome(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostIncome_WithValidModel_ReturnsCreatedAtActionResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var incomeDto = new IncomeDto { Description = "Investment", Amount = 100, Date = DateTime.Now };
            var controller = new IncomesController(context);

            // Act
            var result = await controller.PostIncome(incomeDto);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdIncome = Assert.IsType<IncomeResponseDto>(createdAtActionResult.Value);

            // Assert
            Assert.Equal(incomeDto.Description, createdIncome.Description);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task PostIncome_WithInvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var incomeDto = new IncomeDto { Description = "Salary",Date =DateTime.Now, Amount = -100 }; // Invalid amount
            var controller = new IncomesController(context);
            controller.ModelState.AddModelError("Amount", "Amount must be positive.");

            // Act
            var result = await controller.PostIncome(incomeDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutIncome_WithValidIdAndModel_ReturnsNoContentResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var initialIncome = new Income { Id = 1, Description = "Old Salary", Amount = 1500, Date = DateTime.Now.AddDays(-2) };
            context.Incomes.Add(initialIncome);
            await context.SaveChangesAsync();
            var updatedIncomeDto = new IncomeDto { Description = "New Salary", Amount = 2500, Date = DateTime.Now };
            var controller = new IncomesController(context);

            // Act
            var result = await controller.PutIncome(1, updatedIncomeDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutIncome_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var incomeDto = new IncomeDto { Description = "Salary", Amount = 2000, Date = DateTime.Now };
            var controller = new IncomesController(context);

            // Act - when enter un-exist id=99
            var result = await controller.PutIncome(99, incomeDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutIncome_WithInvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var incomeDto = new IncomeDto {Description = "Profit", Date =DateTime.Now, Amount = -100 }; // Invalid amount
            var controller = new IncomesController(context);
            controller.ModelState.AddModelError("Amount", "Amount must be positive.");

            // Act
            var result = await controller.PutIncome(1, incomeDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteIncome_WithValidId_ReturnsNoContentResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var income = new Income { Id = 1, Description = "Donation", Amount = 50, Date = DateTime.Now.AddDays(-3) };
            context.Incomes.Add(income);
            await context.SaveChangesAsync();
            var controller = new IncomesController(context);

            // Act
            var result = await controller.DeleteIncome(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteIncome_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            await using var context = GetInMemoryDbContext();
            var controller = new IncomesController(context);

            // Act
            var result = await controller.DeleteIncome(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }