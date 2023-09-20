namespace TodoWebService.Models.Entities;

public class TodoItem
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset UpdatedTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Notify { get; set; } = default;
    public string UserId { get; set; } = string.Empty;
    public virtual AppUser? User { get; set; }

}