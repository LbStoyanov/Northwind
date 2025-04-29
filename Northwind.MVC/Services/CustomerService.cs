using Northwind.MVC.Contracts;
using Northwind.MVC.Models;

namespace Northwind.MVC.Services;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetCustomersAsync(string? search = null)
    {
        var url = "/api/customers";
        var customers = await _httpClient.GetFromJsonAsync<IEnumerable<CustomerViewModel>>(url);

        if (!string.IsNullOrWhiteSpace(search))
        {
            customers = customers?
                .Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return customers ?? Enumerable.Empty<CustomerViewModel>();
    }

    public async Task<CustomerViewModel?> GetCustomerByIdAsync(string customerId)
    {
        var url = $"/api/customers/{customerId}";
        var customer = await _httpClient.GetFromJsonAsync<CustomerViewModel>(url);
        return customer;
    }

    public async Task<IEnumerable<OrderViewModel>> GetCustomerOrdersAsync(string customerId)
    {
        var url = $"/api/customers/{customerId}/orders";
        var response = await _httpClient.GetFromJsonAsync<CustomerDetailsViewModel>(url);
        return response?.Orders ?? Enumerable.Empty<OrderViewModel>();
    }
}

