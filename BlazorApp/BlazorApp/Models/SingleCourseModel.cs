namespace BlazorApp.Models;

public class SingleCourseModel
{
    public string Id { get; set; } = null!;
    public string? ImgUrl { get; set; }
    public string? ImgHeaderUrl { get; set; }
    public bool IsDigital { get; set; }
    public bool IsBestseller { get; set; }
    public string[]? Categories { get; set; }
    public string? Title { get; set; }
    public string? Ingress { get; set; }
    public decimal StarRating { get; set; }
    public string? Reviews { get; set; }
    public string? Likes { get; set; }
    public string? LikePercentage { get; set; }
    public string? Hours { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorImgUrl { get; set; }
    public virtual Prices? Prices { get; set; }
    public virtual Content? Content { get; set; }
    
}

public class Prices
{
    public string? Currency { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }

}

public class ProgramDetailItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

}

public class Content
{
    public string? Description { get; set; }
    public string[]? Includes { get; set; }
    public virtual List<ProgramDetailItem>? ProgramDetails { get; set; }

}

