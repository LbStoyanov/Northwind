using Microsoft.AspNetCore.Mvc;
using Northwind.MVC.Services;
using Northwind.MVC.Models;
using Newtonsoft.Json;

namespace Northwind.MVC.Controllers;


public class CustomersController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl = "https://localhost:7128/api/customers"; // Update if needed

    public CustomersController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index(string searchTerm)
    {
        List<CustomerViewModel> customers;

        // If there is a search term, perform the search
        if (!string.IsNullOrEmpty(searchTerm))
        {
            var allCustomersResponse = await _httpClient.GetAsync(_apiBaseUrl);
            if (!allCustomersResponse.IsSuccessStatusCode)
                return StatusCode((int)allCustomersResponse.StatusCode);

            var allCustomersJson = await allCustomersResponse.Content.ReadAsStringAsync();
            var allCustomers = JsonConvert.DeserializeObject<List<CustomerDto>>(allCustomersJson);

            var customer = allCustomers.FirstOrDefault(c =>
                c.ContactName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (customer != null)
            {
                // Return only the found customer
                var singleCustomerViewModel = new CustomerViewModel
                {
                    CustomerId = customer.CustomerId,
                    ContactName = customer.ContactName,
                    OrdersCount = customer.Orders?.Count ?? 0
                };

                customers = new List<CustomerViewModel> { singleCustomerViewModel };
            }
            else
            {
                // If no customer found, return an empty list
                customers = new List<CustomerViewModel>();
            }
        }
        else
        {
            // If there is no search term, return all customers
            var response = await _httpClient.GetAsync(_apiBaseUrl);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();

            // Deserialize DTOs
            var customerDtos = JsonConvert.DeserializeObject<List<CustomerDto>>(json);

            // Map DTOs to ViewModels
            customers = customerDtos.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                ContactName = c.ContactName,
                OrdersCount = c.Orders?.Count ?? 0
            }).ToList();
        }

        return View(customers);
    }

}


