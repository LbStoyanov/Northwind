using Microsoft.AspNetCore.Mvc;
using Northwind.API.Contracts;
using Northwind.API.Data;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        try
        {
            var customers = await _customerService.GetAllCustomersAsync();

            if (customers == null)
            {                
                return StatusCode(500, "An error occurred while retrieving customers.");
            }

            if (!customers.Any())
            {
                return NoContent(); 
            }

            return Ok(customers);
        }
        catch (Exception ex)
        {           
            return StatusCode(500, $"Internal server error occurred: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(string id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {                
                return NotFound();
            }

            return Ok(customer);  
        }
        catch (Exception ex)
        {            
            return StatusCode(500, $"Internal server error occurred: {ex.Message}");
        }
    }

    [HttpGet("{id}/orders")]
    public async Task<IActionResult> GetCustomerOrders(string id)
    {
        try
        {
            var orders = await _customerService.GetCustomerOrdersAsync(id);

            if (orders == null)
            {              
                return NotFound($"Customer with ID {id} not found.");
            }

            if (!orders.Any())
            {
                return NoContent();
            }
           
            return Ok(orders);
        }
        catch (Exception ex)
        {            
            return StatusCode(500, $"Internal server error occurred: {ex.Message}");
        }
    }
}

