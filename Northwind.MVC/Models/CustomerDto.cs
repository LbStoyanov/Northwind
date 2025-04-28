namespace Northwind.MVC.Models
{
    public class CustomerDto
    {
        public string CustomerId { get; set; }
        public string ContactName { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
