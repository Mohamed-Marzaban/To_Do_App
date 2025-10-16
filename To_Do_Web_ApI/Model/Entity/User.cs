namespace To_Do_Web_ApI.Model.Entity;

public class User:BaseEntity
{
    public required string username { get; set; } 
    public required string password { get; set; }
}   