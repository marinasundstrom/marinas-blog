using System.Text.Json;

using ReadTimeEstimator.Implementations.Estimators;

string wwwrootDir = "../PersonalSite/wwwroot/";
string postsDir = $"{wwwrootDir}posts";
string tagsDir = $"{wwwrootDir}tags";

List<PostInfo> posts = new List<PostInfo>();

var htmlEstimator = new HtmlEstimator();

try 
{
    Directory.CreateDirectory(postsDir);
}
catch(Exception) {}

try 
{
    Directory.CreateDirectory(tagsDir);
}
catch(Exception) {}

foreach (var file in Directory.GetFiles(postsDir, "*.json"))
{
    File.Delete(file);
}

foreach (var file in Directory.GetFiles(tagsDir, "*.json"))
{
    File.Delete(file);
}

Dictionary<string, List<PostInfo>> tags = new();

foreach (var file in Directory.GetFiles(postsDir, "*.md"))
{
    Console.WriteLine(file);

    var fileName = Path.GetFileNameWithoutExtension(file);
    var text = await File.ReadAllTextAsync(file);

    var frontMatter = MarkdownExtensions.GetFrontMatter<BlogFrontMatter>(text);

    if (frontMatter is null)
        continue;

    var content = MarkdownExtensions
        .ToHtml(text);

    var excerpt = content.TruncateHtml(500);

    var estimatedTime = TimeSpan.FromMinutes(
        htmlEstimator.ReadTimeInMinutes(content));

    var post = new PostInfo(frontMatter.Title, frontMatter.Subtitle, fileName, frontMatter.Author, frontMatter.Image, frontMatter.Published, estimatedTime, excerpt, frontMatter.Tags);

    posts.Add(post);

    if (frontMatter.Tags is not null)
    {
        foreach (var tag in frontMatter.Tags)
        {
            if (!tags.TryGetValue(tag, out var tagPosts))
            {
                tagPosts = new List<PostInfo>();
                tags[tag] = tagPosts;
            }

            tagPosts.Add(post);
        }
    }
}

var chunks = posts.OrderByDescending(x => x.Published).Chunk(5);

int i = 1;
var count = chunks.Count();

foreach (var chunk in chunks)
{
    var postsJson = JsonSerializer.Serialize(new IndexPage(null, chunk, i, count));

    await File.WriteAllTextAsync($"{postsDir}/_index-{i}.json", postsJson);

    i++;
}

foreach (var tag in tags)
{
    var chunks2 = tag.Value.OrderByDescending(x => x.Published).Chunk(5);

    i = 1;
    count = chunks2.Count();

    foreach (var chunk in chunks2)
    {
        var postsJson = JsonSerializer.Serialize(new IndexPage(tag.Key, chunk, i, count));

        await File.WriteAllTextAsync($"{tagsDir}/{tag.Key}-{i}.json", postsJson);

        i++;
    }
}

foreach(var tag in tags) 
{
    var postsJson = JsonSerializer.Serialize(tags.ToDictionary(x => x.Key, x => x.Value.Count()));

    await File.WriteAllTextAsync($"{tagsDir}/index.json", postsJson);
}

record IndexPage(string? Name, IEnumerable<PostInfo> Posts, int Page, int TotalPages);

record PostInfo(string Title, string? Subtitle, string Slug, string Author, string? Image, DateTime? Published, TimeSpan? EstimatedReadTime, string Excerpt, string[] Tags);