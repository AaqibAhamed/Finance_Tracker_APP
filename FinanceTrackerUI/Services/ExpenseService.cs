using System.Net.Http.Json;
using FinanceTrackerModels.DTOs;

namespace FinanceTrackerUI.Services;

public class ExpenseService
{
    private readonly HttpClient _httpClient;

    public ExpenseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(IEnumerable<GetExpenseDto> Items, int TotalCount, int PageNumber, int PageSize)> GetExpenses(int pageNumber = 1, int pageSize = 10, string? sortBy = null, string? sortDirection = null)
    {
        var query = $"api/Expenses?pageNumber={pageNumber}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(sortBy))
        {
            query += $"&sortBy={sortBy}&sortDirection={sortDirection}";
        }
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<GetExpenseDto>>(query);
        return (response?.Items ?? new List<GetExpenseDto>(), response?.TotalCount ?? 0, response?.PageNumber ?? 1, response?.PageSize ?? 10);
    }

    public async Task<GetExpenseDto?> GetExpense(int id)
    {
        return await _httpClient.GetFromJsonAsync<GetExpenseDto>($"api/Expenses/{id}");
    }

    public async Task<HttpResponseMessage> AddExpense(ExpenseDto expense)
    {
        return await _httpClient.PostAsJsonAsync("api/Expenses", expense);
    }

    public async Task<HttpResponseMessage> UpdateExpense(int id, ExpenseDto expense)
    {
        return await _httpClient.PutAsJsonAsync($"api/Expenses/{id}", expense);
    }

    public async Task<HttpResponseMessage> DeleteExpense(int id)
    {
        return await _httpClient.DeleteAsync($"api/Expenses/{id}");
    }
}