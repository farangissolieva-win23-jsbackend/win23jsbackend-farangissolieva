namespace BlazorApp.Data;

public class UserCourses
{
    public string UserId { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public string CourseId { get; set; } = null!;
    
}
