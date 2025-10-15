using System.ComponentModel.DataAnnotations;

namespace To_Do_Web_ApI.Model.Dto;

public record CreateUserDto(
    [Required]
    string username,
    [Required]
    string password
    );