using OnyxStore.Api.Dtos;
using OnyxStore.Api.Results;

namespace OnyxStore.Api.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(PostUserDto dto);
    Task<AuthResult> LoginAsync(LoginUserDto dto);
}