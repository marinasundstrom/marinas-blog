using System.Text.Json;

string dir = "../PersonalSite/wwwroot/";

List<PostInfo> posts = new List<PostInfo>();

foreach(var file in Directory.GetFiles($"{dir}posts", "*.md"))
{
    Console.WriteLine(file);

    var fileName = Path.GetFileNameWithoutExtension(file);
    var text = await File.ReadAllTextAsync(file);

    var frontMatter = MarkdownExtensions.GetFrontMatter<BlogFrontMatter>(text);

    if(frontMatter is null)
        continue;

    var excerpt = MarkdownExtensions
        .ToHtml(text)
        .TruncateHtml(500);

    posts.Add(new PostInfo(frontMatter.Title, fileName, frontMatter.Author, frontMatter.Image, frontMatter.Published, excerpt, frontMatter.Tags));
}

var postsJson = JsonSerializer.Serialize(posts.OrderByDescending(x => x.Published));

await File.WriteAllTextAsync($"{dir}posts/_index.json", postsJson);

record PostInfo(string Title, string Slug, string Author, string? Image, DateTime Published, string Excerpt, string[] Tags);