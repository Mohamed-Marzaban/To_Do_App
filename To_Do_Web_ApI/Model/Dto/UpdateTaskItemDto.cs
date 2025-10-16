using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace To_Do_Web_ApI.Model.Dto;

public record UpdateTaskItemDto(
    [Optional]
    string title,
    [Optional]
    string description,
    [Optional]
    bool isDone
    );