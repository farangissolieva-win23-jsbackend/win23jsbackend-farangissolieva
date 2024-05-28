namespace BlazorApp.Data;

public class UserMessages
{
    public int Id { get; set; }
    public string SenderId { get; set; } = null!;
    public string RecipientId { get; set; } = null!;
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; }
    
}