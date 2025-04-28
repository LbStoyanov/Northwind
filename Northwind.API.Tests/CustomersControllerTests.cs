using Moq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Controllers;
using Northwind.API.Services;
using Northwind.API.Models;
using Xunit;
using Northwind.API.Contracts;

namespace Northwind.API.Tests
{
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
            // Arrange: Mocking a list of customers
            var customers = new List<Customer>
            {
                new Customer { CustomerId = "ALFKI", CompanyName = "Alfreds Futterkiste" },
                new Customer { CustomerId = "ANATR", CompanyName = "Ana Trujillo Emparedados y helados" }
            };

            _customerServiceMock
                .Setup(service => service.GetAllCustomersAsync())
                .ReturnsAsync(customers);

            // Act: Call the GetAllCustomers method
            var result = await _controller.GetAllCustomers();

            // Assert: Verify the result is Ok and contains customers
            var okResult = Assert.IsType<OkObjectResult>(result); // Should return Ok (HTTP 200)
            var returnedCustomers = Assert.IsType<List<Customer>>(okResult.Value); // Should be a List<Customer>

            Assert.Equal(customers.Count, returnedCustomers.Count); // Verify the number of customers is the same
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsNoContent_WhenNoCustomers()
        {
            // Arrange: Mocking empty customer list
            _customerServiceMock
                .Setup(service => service.GetAllCustomersAsync())
                .ReturnsAsync(new List<Customer>());  // Mocking empty list

            // Act: Call the GetAllCustomers method
            var result = await _controller.GetAllCustomers();

            // Assert: Verify the result is NoContent
            Assert.IsType<NoContentResult>(result); // Should return NoContent (HTTP 204)
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsServerError_WhenServiceReturnsNull()
        {
            // Arrange: Mocking null customer list (simulating error scenario)
            _customerServiceMock
                .Setup(service => service.GetAllCustomersAsync())
                .ReturnsAsync((List<Customer>)null);  // Mocking null response

            // Act: Call the GetAllCustomers method
            var result = await _controller.GetAllCustomers();

            // Assert: Verify the result is InternalServerError (HTTP 500)
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, actionResult.StatusCode);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsNotFound_WhenCustomerNotFound()
        {
            // Arrange: Mocking customer not found by ID
            var customerId = "ALFKI";
            _customerServiceMock
                .Setup(service => service.GetCustomerByIdAsync(customerId))
                .ReturnsAsync((Customer)null);  // Mocking customer not found

            // Act: Call the GetCustomerById method
            var result = await _controller.GetCustomerById(customerId);

            // Assert: Verify the result is NotFound (HTTP 404)
            var actionResult = Assert.IsType<NotFoundResult>(result); // Should return NotFound (HTTP 404)
        }

        [Fact]
        public async Task GetCustomerById_ReturnsCustomer_WhenCustomerFound()
        {
            // Arrange: Mocking customer found
            var customerId = "ALFKI";
            var mockCustomer = new Customer { CustomerId = customerId, CompanyName = "Test Company" };
            _customerServiceMock
                .Setup(service => service.GetCustomerByIdAsync(customerId))
                .ReturnsAsync(mockCustomer);  // Mocking a valid customer

            // Act: Call the GetCustomerById method
            var result = await _controller.GetCustomerById(customerId);

            // Assert: Verify the result is OK (HTTP 200) with the customer data
            var actionResult = Assert.IsType<OkObjectResult>(result); // Should return OK (HTTP 200)
            var returnValue = Assert.IsType<Customer>(actionResult.Value);
            Assert.Equal(customerId, returnValue.CustomerId);
        }

        [Fact]
        public async Task GetCustomerOrders_ReturnsNoContent_WhenNoOrders()
        {
            // Arrange: Mocking no orders for a customer
            var customerId = "ALFKI";
            _customerServiceMock
                .Setup(service => service.GetCustomerOrdersAsync(customerId))
                .ReturnsAsync(new List<Order>());  // Mocking empty orders list

            // Act: Call the GetCustomerOrders method
            var result = await _controller.GetCustomerOrders(customerId);

            // Assert: Verify the result is NoContent (HTTP 204)
            var actionResult = Assert.IsType<NoContentResult>(result); // Should return NoContent (HTTP 204)
        }

        [Fact]
        public async Task GetCustomerOrders_ReturnsServerError_WhenServiceThrowsException()
        {
            // Arrange: Mocking exception scenario in service
            var customerId = "ALFKI";
            _customerServiceMock
                .Setup(service => service.GetCustomerOrdersAsync(customerId))
                .ThrowsAsync(new System.Exception("Test exception")); // Mocking service exception

            // Act: Call the GetCustomerOrders method
            var result = await _controller.GetCustomerOrders(customerId);

            // Assert: Verify the result is InternalServerError (HTTP 500)
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, actionResult.StatusCode);
        }

        [Fact]
        public async Task GetCustomerOrders_ReturnsOrders_WhenOrdersExist()
        {
            // Arrange: Mocking a list of orders
            var customerId = "ALFKI";  // Customer with existing orders
            var orders = new List<Order>
            {
                new Order { OrderId = 1, CustomerId = customerId, OrderDate = DateTime.Now },
                new Order { OrderId = 2, CustomerId = customerId, OrderDate = DateTime.Now.AddDays(-1) }
            };

            _customerServiceMock
                .Setup(service => service.GetCustomerOrdersAsync(customerId))
                .ReturnsAsync(orders);  // Mocking list of orders for the customer

            // Act: Call the GetCustomerOrders method
            var result = await _controller.GetCustomerOrders(customerId);

            // Assert: Verify the result is Ok and contains orders
            var okResult = Assert.IsType<OkObjectResult>(result);  // Should return Ok (HTTP 200)
            var returnedOrders = Assert.IsType<List<Order>>(okResult.Value);  // Should be a List<Order>

            Assert.Equal(orders.Count, returnedOrders.Count);  // Verify the number of orders is the same
        }
    }
}
