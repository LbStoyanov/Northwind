namespace Northwind.API.DTOs;

public class CustomerDTO
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = string.Empty!;

    public int OrdersCount { get; set; }
}

