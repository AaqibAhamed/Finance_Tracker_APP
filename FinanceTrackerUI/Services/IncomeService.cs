using System.Net.Http.Json;
using FinanceTrackerModels.DTOs;

namespace FinanceTrackerUI.Services;

public class IncomeService
{
    private readonly HttpClient _httpClient;

    public IncomeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(IEnumerable<GetIncomeDto> Items, int TotalCount, int PageNumber, int PageSize)> GetIncomes(int pageNumber = 1, int pageSize = 10, string? sortBy = null, string? sortDirection = null)
    {
        var query = $"api/Incomes?pageNumber={pageNumber}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(sortBy))
        {
            query += $"&sortBy={sortBy}&sortDirection={sortDirection}";
        }
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<GetIncomeDto>>(query);
        return (response?.Items ?? new List<GetIncomeDto>(), response?.TotalCount ?? 0, response?.PageNumber ?? 1, response?.PageSize ?? 10);
    }

    public async Task<GetIncomeDto?> GetIncome(int id)
    {
        return await _httpClient.GetFromJsonAsync<GetIncomeDto>($"api/Incomes/{id}");
    }

    public async Task<HttpResponseMessage> AddIncome(IncomeDto income)
    {
        return await _httpClient.PostAsJsonAsync("api/Incomes", income);
    }

    public async Task<HttpResponseMessage> UpdateIncome(int id, IncomeDto income)
    {
        return await _httpClient.PutAsJsonAsync($"api/Incomes/{id}", income);
    }

    public async Task<HttpResponseMessage> DeleteIncome(int id)
    {
        return await _httpClient.DeleteAsync($"api/Incomes/{id}");
    }
}

public class ApiResponse<T>
{
    public IEnumerable<T>? Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}