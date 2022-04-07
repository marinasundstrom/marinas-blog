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

    posts.Add(new PostInfo(frontMatter.Title, frontMatter.Subtitle, fileName, frontMatter.Author, frontMatter.Image, frontMatter.Published, excerpt, frontMatter.Tags));
}

var chunks = posts.OrderByDescending(x => x.Published).Chunk(5);

int i = 1;
var count = chunks.Count();

foreach(var chunk in chunks) 
{
    var postsJson = JsonSerializer.Serialize(new IndexPage(chunk, i, count));

    await File.WriteAllTextAsync($"{dir}posts/_index-{i}.json", postsJson);

    i++;
}

record IndexPage(IEnumerable<PostInfo> Posts, int Page, int TotalPages);

record PostInfo(string Title, string? Subtitle, string Slug, string Author, string? Image, DateTime Published, string Excerpt, string[] Tags);