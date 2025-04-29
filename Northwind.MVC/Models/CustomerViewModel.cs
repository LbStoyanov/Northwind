namespace Northwind.MVC.Models;

public class CustomerViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int OrdersCount { get; set; }
}

