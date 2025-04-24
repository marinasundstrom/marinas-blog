using YamlDotNet.Serialization;

public class BlogFrontMatter
{
    [YamlMember(Alias = "tags")]
    public string[] Tags { get; set; }

    [YamlMember(Alias = "title")]
    public string Title { get; set; }

    [YamlMember(Alias = "subtitle")]
    public string? Subtitle { get; set; }

    [YamlMember(Alias = "author")]
    public string Author { get; set; }

    [YamlMember(Alias = "published")]
    public DateTime? Published { get; set; }

    [YamlMember(Alias = "image")]
    public string Image { get; set; }

    [YamlMember(Alias = "image_credit_name")]
    public string ImageCreditName { get; set; }

    [YamlMember(Alias = "image_credit_url")]
    public string ImageCreditUrl { get; set; }

    [YamlMember(Alias = "image_alt")]
    public string ImageAlt { get; set; }

    [YamlMember(Alias = "redirect_from")]
    public string[] RedirectFrom { get; set; }

    [YamlMember(Alias = "draft")]
    public bool? IsDraft { get; set; }
}