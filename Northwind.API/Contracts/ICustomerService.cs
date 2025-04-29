using Northwind.API.DTOs;

namespace Northwind.API.Contracts;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDTO>?> GetAllCustomersAsync();
    Task<CustomerDTO?> GetCustomerByIdAsync(string id);
    Task<CustomerOrdersDTO?> GetCustomerOrdersAsync(string customerId);
}

