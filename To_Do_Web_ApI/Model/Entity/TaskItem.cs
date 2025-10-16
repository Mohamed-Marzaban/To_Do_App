namespace To_Do_Web_ApI.Model.Entity;

public class TaskItem : BaseEntity
{
    public bool isDone { get; set; } = false;
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}