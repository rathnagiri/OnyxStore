using OnyxStore.Api.Dtos;
using OnyxStore.Api.Models;

namespace OnyxStore.Api.Interfaces;

public interface IProductsService
{
    Task<Product> CreateAsync(ProductDto dto);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetByColorAsync(string color);
}