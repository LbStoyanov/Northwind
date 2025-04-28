using Microsoft.EntityFrameworkCore;
using Northwind.API.Contracts;
using Northwind.API.Models;

namespace Northwind.API.Services;
public class CustomerService : ICustomerService
{
    private readonly INorthwindContext _context;

    public CustomerService(INorthwindContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>?> GetAllCustomersAsync()
    {

        try
        {
            var customers = await _context.Customers.ToListAsync();
            return customers;
        }
        catch (Exception ex)
        {
            // Log the exception in logger mechanism
            Console.WriteLine(ex);
            return null;
        }

    }

    public async Task<Customer?> GetCustomerByIdAsync(string customerId)
    {
        try
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            return customer;
        }
        catch (Exception ex)
        {
            // Log the exception in logger mechanism
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<IEnumerable<Order>?> GetCustomerOrdersAsync(string customerId)
    {

        try
        {
            var customer = await _context.Customers
                                          .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
                return null; 

            var orders = await _context.Orders
                                       .Where(o => o.CustomerId == customerId)
                                       .ToListAsync();

            return orders;
        }
        catch (Exception ex)
        {
            // Log the exception in logger mechanism
            Console.WriteLine(ex);
            return null;
        }
    }
}

