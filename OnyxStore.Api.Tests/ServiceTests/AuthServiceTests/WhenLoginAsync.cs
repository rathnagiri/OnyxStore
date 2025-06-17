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
public class WhenLoginAsync
{
    private AppDbContext _context;
    private Mock<IPasswordHasher<User>> _passwordHasherMock;
    private AuthService _authService;

    private LoginUserDto _user;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "AuthTestDb")
            .Options;
        _context = new AppDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _user = new LoginUserDto()
        {
            Email = "abc@abc.om",
            Password = "pass123"
        };

        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _authService = new AuthService(_context, _passwordHasherMock.Object);
    }

    [TestMethod]
    public async Task Given_EmailNotFound_Then_ReturnsInvalid_()
    {
        var dto = new LoginUserDto(){
            Email = "nouser@example.com", 
            Password = "pass"};
        var result = await _authService.LoginAsync(dto);

        result.Status.ShouldBe(AuthStatusResult.InvalidEmailOrPassword);
        result.Token.ShouldBeNull();
    }
    
    [TestMethod]
    public async Task Given_PasswordIsWrong_Then_ReturnsInvalid()
    {
        var user = new User { Email = "test2@example.com", Password = "hash" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _passwordHasherMock
            .Setup(h => h.VerifyHashedPassword(user, user.Password, "wrongpass"))
            .Returns(PasswordVerificationResult.Failed);

        var dto = new LoginUserDto() {
            Email = "test2@example.com", 
            Password = "wrongpass"};
        var result = await _authService.LoginAsync(dto);

        result.Status.ShouldBe(AuthStatusResult.InvalidEmailOrPassword);
        result.Token.ShouldBeNull();
    }
    
    [TestMethod]
    public async Task Given_CredentialsAreValid_Then_ReturnsValid()
    {
        var user = new User { Email = "login@example.com", Password = "hashedpwd" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _passwordHasherMock
            .Setup(h => h.VerifyHashedPassword(user, user.Password, "password123"))
            .Returns(PasswordVerificationResult.Success);

        var dto = new LoginUserDto() {
            Email = "login@example.com", 
            Password = "password123"};
        var result = await _authService.LoginAsync(dto);

        result.Status.ShouldBe(AuthStatusResult.Success);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

}