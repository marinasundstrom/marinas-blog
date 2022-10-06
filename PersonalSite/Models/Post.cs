namespace PersonalSite.Models;

public class Post 
{
    public string Title { get; set; }

    public string? Subtitle { get; set; }
    
    public string Slug { get; set; }

    public string[] Tags { get; set; }

    public string Author { get; set; }

    public DateTime? Published { get; set; }

    public DateTime? LastModified { get; set; }

    public string? Image { get; set; }

    public string? ImageAlt { get; set; }

    public string? ImageCredit { get; set; }

    public string Content { get; set; }
}
