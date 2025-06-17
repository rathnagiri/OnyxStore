using Microsoft.EntityFrameworkCore;
using OnyxStore.Api.Data;
using OnyxStore.Api.Dtos;
using OnyxStore.Api.Models;
using OnyxStore.Api.Services;
using Shouldly;

namespace OnyxStore.Api.Tests.ServiceTests.ProductServiceTests;

[TestClass]
public class WhenProductService
{
    private AppDbContext _context;
        private ProductsService _productsService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("ProductsDb")
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _productsService = new ProductsService(_context);
        }

        [TestMethod]
        public async Task Given_ProductDto_When_CreateAsync_ShouldAddProduct()
        {
            var dto = new ProductDto
            {
                Name = "T-Shirt",
                Color = "Red",
                Category = "Apparel",
                Description = "Red Cotton T-Shirt",
                Price = 20
            };

            var result = await _productsService.CreateAsync(dto);

            result.ShouldNotBeNull();
            result.Id.ShouldBeGreaterThan(0);
            result.Name.ShouldBe(dto.Name);

            var productInDb = await _context.Products.FirstOrDefaultAsync();
            productInDb.ShouldNotBeNull();
            productInDb.Name.ShouldBe("T-Shirt");
        }

        [TestMethod]
        public async Task When_GetAllAsync_ShouldReturnAllProducts()
        {
            _context.Products.AddRange(new List<Product>
            {
                new() { Name = "Shirt", Color = "Blue", Category = "Apparel", Description = "Blue Shirt", Price = 25 },
                new() { Name = "Hat", Color = "Black", Category = "Accessories", Description = "Black Hat", Price = 15 }
            });
            await _context.SaveChangesAsync();

            var products = await _productsService.GetAllAsync();

            products.Count.ShouldBe(2);
        }

        [TestMethod]
        public async Task When_GetByColorAsync_ShouldReturnOnlyMatchingColor()
        {
            _context.Products.AddRange(new List<Product>
            {
                new() { Name = "Shirt", Color = "Red", Category = "Apparel", Description = "Red Shirt", Price = 25 },
                new() { Name = "Jacket", Color = "Blue", Category = "Apparel", Description = "Blue Jacket", Price = 100 },
                new() { Name = "Cap", Color = "Red", Category = "Accessories", Description = "Red Cap", Price = 15 }
            });
            await _context.SaveChangesAsync();

            var redProducts = await _productsService.GetByColorAsync("Red");

            redProducts.Count.ShouldBe(2);
            redProducts.All(p => p.Color == "Red").ShouldBeTrue();
        }
    }
    
