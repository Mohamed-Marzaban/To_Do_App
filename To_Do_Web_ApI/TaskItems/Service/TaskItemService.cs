using Microsoft.EntityFrameworkCore;
using To_Do_Web_ApI.Data;
using To_Do_Web_ApI.Model.Dto;
using To_Do_Web_ApI.Model.Entity;

namespace To_Do_Web_ApI.TaskItems.Service;

public class TaskItemService
{
    private readonly ApplicationDbContext _db;

    public TaskItemService(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<TaskItem> CreateTaskItemAsync(CreateTaskItemDto taskDto,int userId)
    {
        TaskItem task = new()
        {
            Title = taskDto.title,
            Description = taskDto.description,
            UserId = userId
        };

        await _db.TaskItems.AddAsync(task);
        await _db.SaveChangesAsync();
        return task;

    }

    public async Task<List<TaskItem>> GetAllTasksAsync(int userId)
    {
       return await this._db.TaskItems.Where(task=>task.UserId==userId).ToListAsync();
    }

    public async Task<TaskItem?> GetTaskItemByIdAsync(int taskId,int userId)
    {
        return await this._db.TaskItems.Where(task => task.Id == taskId && task.UserId==userId).FirstOrDefaultAsync();
    }
    
    public async Task DeleteTaskItemAsync(TaskItem taskItem)
    { 
        this._db.TaskItems.Remove(taskItem);
        await _db.SaveChangesAsync();

    }

    public async Task<TaskItem?> UpdateTaskItemAsync(UpdateTaskItemDto taskDto,TaskItem task)
    {
        
        task.Title = taskDto.title ?? task.Title;
        task.Description = taskDto.description ?? task.Description;
        task.isDone = taskDto.isDone ?? task.isDone;

        await _db.SaveChangesAsync();

        return task;


    }
    
}