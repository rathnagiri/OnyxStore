using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;
using OnyxStore.Api.Controllers;
using OnyxStore.Api.Dtos;
using OnyxStore.Api.Interfaces;
using OnyxStore.Api.Results;
using OnyxStore.Api.Services;
using Shouldly;

namespace OnyxStore.Api.Tests.ControllerTests.AuthControllerTests;

[TestClass]
public class WhenRegister
{
    private Mock<IAuthService> _mockAuthService;
    private AuthController _controller;
    private PostUserDto _dto;
    
    [TestInitialize]
    public void Setup()
    {
        _mockAuthService = new Mock<IAuthService>(MockBehavior.Strict);
        _controller = new AuthController(_mockAuthService.Object);
        _dto = new PostUserDto()
        {
            Email = "test@example.com",
            Password = "password123"
        };
    }
    
    [TestMethod]
    public async Task GivenSuccess_Call_Then_ReturnOk()
    {
        var token = "sample_jwt_token";
        _mockAuthService.Setup(s => s.RegisterAsync(_dto)).ReturnsAsync(new AuthResult
        {
            Status = AuthStatusResult.Success,
            Token = token
        });

        var result = await _controller.Register(_dto);

        result.ShouldBeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.ShouldBeAssignableTo(typeof(object));
        okResult.Value.ToString().ShouldContain(token);
    }

    [TestMethod]
    public async Task Given_Invalid_Email_ModelState_Error_Then_ReturnBadRequest()
    {
        _dto.Email = "abc";
        _dto.Password = "pas";
        
        _controller.ModelState.AddModelError("Email", "The Email field is not a valid e-mail address.");
        var result = await _controller.Register(_dto);
        
        result.ShouldBeOfType<BadRequestObjectResult>();

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.ShouldNotBeNull();
        badRequestResult.StatusCode.ShouldBe(400);
        badRequestResult.Value.ToJson().ShouldContain("Email");
    }
    
    [TestMethod]
    public async Task Given_BadRequest_WhenEmailExists_Then_ReturnBadRequest()
    {
        _mockAuthService.Setup(s => s.RegisterAsync(_dto)).ReturnsAsync(new AuthResult
        {
            Status = AuthStatusResult.EmailAlreadyInUse
        });

        var result = await _controller.Register(_dto);

        result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequest = result as BadRequestObjectResult;
        badRequest!.Value.ToString().ShouldContain(AuthStatusResult.EmailAlreadyInUse.ToString());
    }
}