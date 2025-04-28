using Northwind.MVC.Models;

namespace Northwind.MVC.Services;

public class CustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetCustomersAsync(string? search = null)
    {
        // Adjust API URL if you have a search endpoint later.
        var url = "/api/customers";

        var customers = await _httpClient.GetFromJsonAsync<IEnumerable<CustomerViewModel>>(url);

        if (!string.IsNullOrEmpty(search))
        {
            customers = customers?.Where(c => c.ContactName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return customers ?? Enumerable.Empty<CustomerViewModel>();
    }
}

