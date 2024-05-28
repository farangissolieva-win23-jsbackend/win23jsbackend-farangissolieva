using static BlazorApp.Components.Pages.Courses;

namespace BlazorApp.Models;

public class CourseModel
{
     public string Id { get; set; } = null!;
    public string? ImgUrl { get; set; }
    public bool IsBestseller { get; set; }
    public string? Title { get; set; }
    public string? Ingress { get; set; }
    public string? Reviews { get; set; }
    public string? Likes { get; set; }
    public string? LikePercentage { get; set; }
    public string? Hours { get; set; }
    public string AuthorName { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public Category? Category { get; set; }
    
}

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }

}