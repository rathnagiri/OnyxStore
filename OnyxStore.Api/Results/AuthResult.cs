namespace OnyxStore.Api.Results;

public class AuthResult
{
    public AuthStatusResult Status { get; set; }
    public string Token { get; set; } // Only set if Status == Success
}