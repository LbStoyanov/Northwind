using Microsoft.EntityFrameworkCore;
using Northwind.API.Contracts;
using Northwind.API.DTOs;

namespace Northwind.API.Services;
public class CustomerService : ICustomerService
{
    private readonly INorthwindContext _context;

    public CustomerService(INorthwindContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerDTO>?> GetAllCustomersAsync()
    {

        try
        {
            var customerDtos = await _context.Customers
           .Select(c => new CustomerDTO
           {
               Id = c.CustomerId,
               Name = c.ContactName ?? "N/A",
               OrdersCount = c.Orders.Count
           })
           .ToListAsync();

            return customerDtos;
        }
        catch (Exception ex)
        {
            // Log the exception in logger mechanism
            Console.WriteLine(ex);
            return null;
        }

    }

    public async Task<CustomerDTO?> GetCustomerByIdAsync(string customerId)
    {
        try
        {
            var customerDto = await _context.Customers
            .Where(c => c.CustomerId == customerId)
            .Select(c => new CustomerDTO
            {
                Id = c.CustomerId,
                Name = string.IsNullOrWhiteSpace(c.ContactName) ? "N/A" : c.ContactName,
                OrdersCount = c.Orders.Count
            })
            .FirstOrDefaultAsync();

            return customerDto;
        }
        catch (Exception ex)
        {
            // Log the exception in logger mechanism
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<CustomerOrdersDTO?> GetCustomerOrdersAsync(string customerId)
    {

        try
        {
            var customerOrders = await _context.Customers
            .Where(c => c.CustomerId == customerId)
            .Select(c => new CustomerOrdersDTO
            {
                CustomerId = c.CustomerId,
                CustomerName = string.IsNullOrWhiteSpace(c.ContactName) ? "N/A" : c.ContactName,
                Orders = c.Orders.Select(o => new OrderSummaryDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalAmount = decimal
                    .Parse(Math.Round(o.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)), 2)
                    .ToString("0.00")),

                    NumberOfProducts = o.OrderDetails.Count,

                    WarningMessage = o.OrderDetails
                        .Any(od =>
                            od.Product.Discontinued ||
                            od.Product.UnitsInStock < od.Product.UnitsOnOrder)
                        ? "Possible execution problem: discontinued or low stock."
                        : null
                }).ToList()
            })
            .FirstOrDefaultAsync();

            return customerOrders;
        }
        catch (Exception ex)
        {
            // Log the exception in logger mechanism
            Console.WriteLine(ex);
            return null;
        }
    }
}

