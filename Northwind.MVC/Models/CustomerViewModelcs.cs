namespace Northwind.MVC.Models;
public class CustomerViewModel
{
    public string CustomerId { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public int OrdersCount { get; set; }
}

