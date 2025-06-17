using Microsoft.AspNetCore.Mvc;
using Moq;
using OnyxStore.Api.Controllers;
using OnyxStore.Api.Dtos;
using OnyxStore.Api.Interfaces;
using OnyxStore.Api.Models;
using Shouldly;

namespace OnyxStore.Api.Tests.ControllerTests.ProductControllerTests;

[TestClass]
public class WhenProductController
{
    private Mock<IProductsService> _mockProductService;
    private ProductController _controller;
    private ProductDto _dto;
    private Product _product;

    [TestInitialize]
    public void Setup()
    {
        _mockProductService = new Mock<IProductsService>(MockBehavior.Strict);
        _controller = new ProductController(_mockProductService.Object);
        _dto = new ProductDto
        {
            Name = "Shirt",
            Color = "Red",
            Category = "Apparel",
            Description = "Red Cotton Shirt",
            Price = 29.99M,
        };
        _product = new Product()
        {
            Name = "Shirt",
            Color = "Red",
            Category = "Apparel",
            Description = "Red Cotton Shirt",
            Price = 29.99M,
        };
    }
    
    [TestMethod]
    public async Task Given_Success_Scenario_Then_Return_OK_WithCreatedProduct()
    {
        var expected = _product;
        _mockProductService.Setup(s => s.CreateAsync(_dto)).ReturnsAsync(expected);

        var result = await _controller.Create(_dto);

        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBe(expected);
    }
    
    [TestMethod]
    public async Task When_GetAll_Then_ReturnsOk_WithProductList()
    {
        var products = new List<Product>
        {
            _product,
            _product
        };
        _mockProductService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        var result = await _controller.GetAll();

        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBe(products);
    }

    [TestMethod]
    public async Task When_GetByColor_Tehn_ReturnsOk_WithMatchingProducts()
    {
        const string color = "Red";
        List<Product> redProducts = [_product];
        _mockProductService.Setup(s => s.GetByColorAsync(color)).ReturnsAsync(redProducts);
        
        var result = await _controller.GetByColor(color);

        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBe(redProducts);
    }
    
    [TestMethod]
    public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        var mockService = new Mock<IProductsService>();
        var controller = new ProductController(mockService.Object);

        controller.ModelState.AddModelError("Name", "The Name field is required.");

        var invalidDto = new ProductDto
        {
            // Name is missing or too short (simulate it by not setting it)
            Color = "Red"
        };

        var result = await controller.Create(invalidDto);

        // Assert
        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;

        badRequestResult.ShouldNotBeNull();
        badRequestResult.StatusCode.ShouldBe(400);
        badRequestResult.Value.ToString().ShouldContain("Validation failed");
    }
}