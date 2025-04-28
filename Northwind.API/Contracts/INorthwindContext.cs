using Microsoft.EntityFrameworkCore;
using Northwind.API.Models;

namespace Northwind.API.Contracts;

public interface INorthwindContext
{
    DbSet<Customer> Customers { get; set; }

    DbSet<Order> Orders { get; set; }
    
}

