using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using OnyxStore.Api.Data;
using OnyxStore.Api.Dtos;
using OnyxStore.Api.Models;
using OnyxStore.Api.Services;
using Shouldly;

namespace OnyxStore.Api.Tests.ServiceTests.AuthServiceTests;

[TestClass]
public class WhenRegisterAsync
{
    private AppDbContext _context;
    private Mock<IPasswordHasher<User>> _passwordHasherMock;
    private AuthService _authService;
    private ProductDto _dto;
    private User _user;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "AuthTestDb")
            .Options;
        _context = new AppDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _dto = new ProductDto
        {
            Name = "Shirt",
            Color = "Red",
            Category = "Apparel",
            Description = "Red Cotton Shirt",
            Price = 29.99M,
        };
        _user = new User()
        {
            Email = "abc@abc.om",
            Password = "pass123"
        };

        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _authService = new AuthService(_context, _passwordHasherMock.Object);
    }

    [TestMethod]
    public async Task Given_NewUserRegistered_Then_Return_Success()
    {
        _passwordHasherMock.Setup(h => h.HashPassword(null, _user.Password)).Returns("hashedpassword");

        var result = await _authService.RegisterAsync(_user);

        result.Status.ShouldBe(AuthStatusResult.Success);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public async Task Given_EmailAlreadyInUse_WhenEmailExists_Then_Return_Error()
    {
        var existingUser = new User { Email = "test@example.com", Password = "pwd" };
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var dto = new PostUserDto() {Email = "test@example.com", Password = "password123"};

        var result = await _authService.RegisterAsync(dto);

        result.Status.ShouldBe(AuthStatusResult.EmailAlreadyInUse);
        result.Token.ShouldBeNull();
    }
}