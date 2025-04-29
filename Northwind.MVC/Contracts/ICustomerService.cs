using Northwind.MVC.Models;

namespace Northwind.MVC.Contracts;

public interface ICustomerService
{
    Task<IEnumerable<CustomerViewModel>> GetCustomersAsync(string? search = null);
    Task<CustomerViewModel?> GetCustomerByIdAsync(string customerId);
    Task<IEnumerable<OrderViewModel>> GetCustomerOrdersAsync(string customerId);
}

