using Northwind.API.Models;

namespace Northwind.API.Contracts
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>?> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(string id);
        Task<IEnumerable<Order>?> GetCustomerOrdersAsync(string customerId);
    }
}
