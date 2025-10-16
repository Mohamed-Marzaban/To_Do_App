using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;

namespace To_Do_Web_ApI.Model.Dto;

public record CreateTaskItemDto(
    [Required]
    string title,
    [Optional]
    string description
    );