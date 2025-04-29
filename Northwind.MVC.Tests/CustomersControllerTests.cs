using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.MVC.Contracts;
using Northwind.MVC.Controllers;
using Northwind.MVC.Models;


namespace Northwind.MVC.Tests;

public class CustomersControllerTests
{
    private readonly Mock<ICustomerService> _mockService;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _mockService = new Mock<ICustomerService>();
        _controller = new CustomersController(_mockService.Object);
    }

    [Fact]
    public async Task Index_NoSearchTerm_ReturnsAllCustomers()
    {
        
        var customers = new List<CustomerViewModel>
        {
            new CustomerViewModel { Id = "ALFKI", Name = "Maria Anders", OrdersCount = 5 }
        };

        _mockService.Setup(s => s.GetCustomersAsync(null))
                    .ReturnsAsync(customers);
       
        var result = await _controller.Index(null) as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(result.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Details_CustomerExists_ReturnsViewResult()
    {

        var customerId = "ALFKI";
        var customer = new CustomerViewModel { Id = customerId, Name = "Maria Anders" };
        var orders = new List<OrderViewModel>
        {
            new OrderViewModel { OrderId = 1, NumberOfProducts = 2, TotalAmount = 100.0m }
        };

        _mockService.Setup(s => s.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);
        _mockService.Setup(s => s.GetCustomerOrdersAsync(customerId)).ReturnsAsync(orders);

   
        var result = await _controller.Details(customerId) as ViewResult;


        Assert.NotNull(result);
        var model = Assert.IsType<CustomerDetailsViewModel>(result.Model);
        Assert.Equal(customerId, model.CustomerId);
        Assert.Single(model.Orders);
    }

    [Fact]
    public async Task Details_CustomerDoesNotExist_ReturnsNotFound()
    {
    
        _mockService.Setup(s => s.GetCustomerByIdAsync("UNKNOWN")).ReturnsAsync((CustomerViewModel?)null);

    
        var result = await _controller.Details("UNKNOWN");

       
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Index_WithSearchTerm_FiltersCustomers()
    {
        
        var customers = new List<CustomerViewModel>
        {
            new CustomerViewModel { Id = "1", Name = "Alice" ,OrdersCount = 5},
            new CustomerViewModel { Id = "2", Name = "Bob" , OrdersCount = 5}
        };

        _mockService.Setup(s => s.GetCustomersAsync("Alice")).ReturnsAsync(customers.Where(c => c.Name == "Alice"));

    
        var result = await _controller.Index("Alice") as ViewResult;

   
        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(result.Model);
        Assert.Single(model);
        Assert.Equal("Alice", model.First().Name);

        Assert.Equal(5, customers.First().OrdersCount);
        Assert.Equal(5, customers[1].OrdersCount);

    }

    [Fact]
    public async Task Details_EmptyCustomerId_ReturnsNotFound()
    {
        
        var result = await _controller.Details(string.Empty);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_CustomerWithNoOrders_ReturnsEmptyOrderList()
    {
       
        var customerId = "ALFKI";
        var customer = new CustomerViewModel { Id = customerId, Name = "Maria Anders" };

        _mockService.Setup(s => s.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);
        _mockService.Setup(s => s.GetCustomerOrdersAsync(customerId)).ReturnsAsync(Enumerable.Empty<OrderViewModel>());

       
        var result = await _controller.Details(customerId) as ViewResult;

        
        Assert.NotNull(result);
        var model = Assert.IsType<CustomerDetailsViewModel>(result.Model);
        Assert.Equal(customerId, model.CustomerId);
        Assert.Empty(model.Orders);
    }

    [Fact]
    public async Task Index_SetsSearchTermInViewBag()
    {
        
        var searchTerm = "test";
        _mockService.Setup(s => s.GetCustomersAsync(searchTerm)).ReturnsAsync(new List<CustomerViewModel>());

       
        var result = await _controller.Index(searchTerm) as ViewResult;

        
        Assert.NotNull(result);
        Assert.Equal(searchTerm, _controller.ViewBag.Search);
    }

}
