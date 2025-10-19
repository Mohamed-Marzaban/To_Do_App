using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using To_Do_Web_ApI.Model.Dto;
using To_Do_Web_ApI.Model.Entity;
using To_Do_Web_ApI.TaskItems.Service;

namespace To_Do_Web_ApI.TaskItems;

[ApiController]
[Authorize]
[Route("api/task")]
public class TaskItemController : ControllerBase
{
    private readonly TaskItemService _taskItemService;

    public TaskItemController(TaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTaskItemAsync(CreateTaskItemDto taskDto)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value!);
        TaskItem task = await this._taskItemService.CreateTaskItemAsync(taskDto, userId);
        return Created("Created Task",task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasksAsync()
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value!);
        List<TaskItem> tasks = await this._taskItemService.GetAllTasksAsync(userId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskItemByIdAsync(int id)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value!);
        TaskItem? task = await this._taskItemService.GetTaskItemByIdAsync(id, userId);
        
        return task is not null?Ok(task):NotFound();
    }
    

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTaskItemAsync(int id, UpdateTaskItemDto taskDto)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value!);
        TaskItem? task = await this._taskItemService.GetTaskItemByIdAsync(id,userId);
        
        if (task is null)
            return NotFound();
        
        task = await this._taskItemService.UpdateTaskItemAsync(taskDto,task);
        
        return task is not null ? Ok(new{message ="Updated Task",updatedTask = task}):NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskItemAsync(int id)
    {
        int userId = int.Parse(User.FindFirst("userId")?.Value!);
        TaskItem? task = await this._taskItemService.GetTaskItemByIdAsync(id,userId);
        
        if (task is null)
            return NotFound();
    
        await this._taskItemService.DeleteTaskItemAsync(task);
        
        return Ok(new { message = "Task deleted", deletedTask = task });
    }
    
}