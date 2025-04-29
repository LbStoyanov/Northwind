using Moq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Controllers;
using Northwind.API.Contracts;
using Northwind.API.DTOs;

namespace Northwind.API.Tests;

public class CustomersControllerTests
{
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _customerServiceMock = new Mock<ICustomerService>();
        _controller = new CustomersController(_customerServiceMock.Object);
    }

    [Fact]
    public async Task GetAllCustomers_ReturnsCustomers_WhenCustomersExist()
    {
        
        var customers = new List<CustomerDTO>
            {
                new CustomerDTO { Id = "ALFKI", Name = "Alfreds Futterkiste" },
                new CustomerDTO { Id = "ANATR", Name = "Ana Trujillo Emparedados y helados" }
            };

        _customerServiceMock
            .Setup(service => service.GetAllCustomersAsync())
            .ReturnsAsync(customers);

        
        var result = await _controller.GetAllCustomers();

        
        var okResult = Assert.IsType<OkObjectResult>(result); 
        var returnedCustomers = Assert.IsType<List<CustomerDTO>>(okResult.Value); 

        Assert.Equal(customers.Count, returnedCustomers.Count);
    }

    [Fact]
    public async Task GetAllCustomers_ReturnsNoContent_WhenNoCustomers()
    {
       
        _customerServiceMock
            .Setup(service => service.GetAllCustomersAsync())
            .ReturnsAsync(new List<CustomerDTO>());

       
        var result = await _controller.GetAllCustomers();

       
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetAllCustomers_ReturnsServerError_WhenServiceReturnsNull()
    {
        
        _customerServiceMock
            .Setup(service => service.GetAllCustomersAsync())
            .ReturnsAsync((List<CustomerDTO>)null);  

        
        var result = await _controller.GetAllCustomers();

        
        var actionResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.InternalServerError, actionResult.StatusCode);
    }

    [Fact]
    public async Task GetCustomerById_ReturnsNotFound_WhenCustomerNotFound()
    {
       
        var customerId = "ALFKI";
        _customerServiceMock
            .Setup(service => service.GetCustomerByIdAsync(customerId))
            .ReturnsAsync((CustomerDTO)null); 

       
        var result = await _controller.GetCustomerById(customerId);

        
        var actionResult = Assert.IsType<NotFoundResult>(result); 
    }

    [Fact]
    public async Task GetCustomerById_ReturnsCustomer_WhenCustomerFound()
    {
        
        var customerId = "ALFKI";
        var mockCustomer = new CustomerDTO { Id = customerId, Name = "Test Company", OrdersCount = 5 };
        _customerServiceMock
            .Setup(service => service.GetCustomerByIdAsync(customerId))
            .ReturnsAsync(mockCustomer);  

        
        var result = await _controller.GetCustomerById(customerId);

        
        var actionResult = Assert.IsType<OkObjectResult>(result); 
        var returnValue = Assert.IsType<CustomerDTO>(actionResult.Value);
        Assert.Equal(customerId, returnValue.Id);
    }

    [Fact]
    public async Task GetCustomerOrders_ReturnsNoContent_WhenNoOrders()
    {
        
        var customerId = "ALFKI";
        _customerServiceMock
            .Setup(service => service.GetCustomerOrdersAsync(customerId))
            .ReturnsAsync(new CustomerOrdersDTO());  
        
        var result = await _controller.GetCustomerOrders(customerId);

        
        var actionResult = Assert.IsType<OkObjectResult>(result); 
    }

    [Fact]
    public async Task GetCustomerOrders_ReturnsServerError_WhenServiceThrowsException()
    {
        
        var customerId = "ALFKI";
        _customerServiceMock
            .Setup(service => service.GetCustomerOrdersAsync(customerId))
            .ThrowsAsync(new System.Exception("Test exception")); 

        
        var result = await _controller.GetCustomerOrders(customerId);

       
        var actionResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.InternalServerError, actionResult.StatusCode);
    }

    [Fact]
    public async Task GetCustomerOrders_ReturnsOrders_WhenOrdersExist()
    {
        
        var customerId = "ALFKI";  
        var mockCustomerOrders = new CustomerOrdersDTO
        {
            CustomerId = customerId,
            CustomerName = "Alfreds Futterkiste",
            Orders = new List<OrderSummaryDTO>
                            {
                                new OrderSummaryDTO
                                {
                                    OrderId = 1,
                                    OrderDate = DateTime.Now,
                                    TotalAmount = 100.00m,
                                    NumberOfProducts = 2,
                                    WarningMessage = null
                                },
                                new OrderSummaryDTO
                                {
                                    OrderId = 2,
                                    OrderDate = DateTime.Now.AddDays(-1),
                                    TotalAmount = 200.00m,
                                    NumberOfProducts = 3,
                                    WarningMessage = "Possible execution problem: discontinued or low stock."
                                }
                            }
        };

        _customerServiceMock
            .Setup(service => service.GetCustomerOrdersAsync(customerId))
            .ReturnsAsync(mockCustomerOrders);

        
        var result = await _controller.GetCustomerOrders(customerId);

        
        var okResult = Assert.IsType<OkObjectResult>(result);  
        var returnedOrders = Assert.IsType<CustomerOrdersDTO>(okResult.Value);  

        Assert.Equal(mockCustomerOrders.Orders.Count, returnedOrders.Orders.Count);  
    }
}

