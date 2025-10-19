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
        int userId = int.Parse(User.FindFirst("userId")?.Value);
        TaskItem task = await this._taskItemService.CreateTaskItemAsync(taskDto, userId);
        return Created("Created Task",task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasksAsync()
    {
        List<TaskItem> tasks = await this._taskItemService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTaskItemByIdAsync(int taskId)
    {
        TaskItem? task = await this._taskItemService.GetTaskItemByIdAsync(taskId);
        
        return task is not null?Ok(task):NotFound();
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetTaskItemByUserIdAsync(int userId)
    {
        List<TaskItem> tasks = await this._taskItemService.GetTaskItemsByUserIdAsync(userId);
        
        return Ok(tasks);
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTaskItemAsync(int taskId, UpdateTaskItemDto taskDto)
    {
        TaskItem? task = await this._taskItemService.GetTaskItemByIdAsync(taskId);
        if (task is null)
            return NotFound();
        if(task.UserId != int.Parse(User.FindFirst("userId")?.Value!))
            return Unauthorized();
        
        task = await this._taskItemService.UpdateTaskItemAsync(taskDto,taskId,task);
        
        return task is not null ? Ok(new{message ="Updated Task",updatedTask = task}):NotFound();
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTaskItemAsync(int taskId)
    {
        TaskItem? task = await this._taskItemService.GetTaskItemByIdAsync(taskId);
        if (task is null)
            return NotFound();
        if(task.UserId != int.Parse(User.FindFirst("userId")?.Value!))
            return Unauthorized();
        await this._taskItemService.DeleteTaskItemAsync(task);
        
        return Ok(new { message = "Task deleted", deletedTask = task });
    }
    
}