namespace Northwind.MVC.Models;

public class CustomerDetailsViewModel
{
    public string CustomerId { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public List<OrderViewModel> Orders { get; set; } = new();
}
