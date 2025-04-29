namespace Northwind.API.DTOs;
public class CustomerOrdersDTO
{
    public string CustomerId { get; set; } = null!;
    public string CustomerName { get; set; } = "N/A";
    public List<OrderSummaryDTO> Orders { get; set; } = new();
}

