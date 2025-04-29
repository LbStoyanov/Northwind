using Microsoft.AspNetCore.Mvc;
using Northwind.MVC.Contracts;
using Northwind.MVC.Models;


namespace Northwind.MVC.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(string? searchTerm)
    {
        var customers = await _customerService.GetCustomersAsync(searchTerm);
        ViewBag.Search = searchTerm;
        return View(customers);
    }

    public async Task<IActionResult> Details(string id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        var orders = await _customerService.GetCustomerOrdersAsync(id);

        if (customer == null)
            return NotFound();

        var viewModel = new CustomerDetailsViewModel
        {
            CustomerId = customer.Id,
            ContactName = customer.Name,
            Orders = orders.ToList()
        };

        return View(viewModel);
    }
}
