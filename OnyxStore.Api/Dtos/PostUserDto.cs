using System.ComponentModel.DataAnnotations;

namespace OnyxStore.Api.Dtos;

public class PostUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    [MaxLength(20)]
    public string Password { get; set; }
}