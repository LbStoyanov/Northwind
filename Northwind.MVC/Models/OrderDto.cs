namespace Northwind.MVC.Models
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public decimal? Total { get; set; }
        public int ProductCount { get; set; }
    }
}
