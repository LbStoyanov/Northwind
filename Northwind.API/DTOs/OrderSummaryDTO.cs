namespace Northwind.API.DTOs;
public class OrderSummaryDTO
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int NumberOfProducts { get; set; }
    public string? WarningMessage { get; set; }
}

