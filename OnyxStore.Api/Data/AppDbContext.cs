using Microsoft.EntityFrameworkCore;
using OnyxStore.Api.Models;

namespace OnyxStore.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
}