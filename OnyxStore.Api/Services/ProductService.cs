using Microsoft.EntityFrameworkCore;
using OnyxStore.Api.Data;
using OnyxStore.Api.Dtos;
using OnyxStore.Api.Interfaces;
using OnyxStore.Api.Models;

namespace OnyxStore.Api.Services;

public class ProductsService(AppDbContext dbContext) : IProductsService
{
    public async Task<Product> CreateAsync(ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Color = dto.Color,
            Category = dto.Category,
            Description = dto.Description,
            Price = dto.Price
        };
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        
        return product;
    }

    public async Task<List<Product>> GetAllAsync() => await dbContext.Products.ToListAsync();

    public async Task<List<Product>> GetByColorAsync(string color) =>
        await dbContext.Products.Where(p => p.Color == color).ToListAsync();
}