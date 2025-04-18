namespace PersonalSite.Markdown;

using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

public class NestedSectionRenderer : HtmlObjectRenderer<MarkdownDocument>
{
    protected override void Write(HtmlRenderer renderer, MarkdownDocument document)
    {
        var sectionTree = ParseSections(document.ToArray());
        foreach (var section in sectionTree)
            RenderSection(renderer, section);
    }

    static void RenderSection(HtmlRenderer renderer, SectionBlock section)
    {
        var rawId = section.Heading.GetAttributes().Id;
        var id = SanitizeId(rawId); // <- sanitize here

        renderer.WriteLine($"<section id=\"{id}\">");

        // Temporarily remove the id attribute from the heading before rendering
        var attributes = section.Heading.GetAttributes();
        var originalId = attributes.Id;
        attributes.Id = null;

        renderer.Render(section.Heading);

        // Restore it just in case (probably not needed unless reused elsewhere)
        attributes.Id = originalId;

        foreach (var block in section.Content)
            renderer.Render(block);

        foreach (var child in section.Children)
            RenderSection(renderer, child);

        renderer.WriteLine("</section>");
    }

    static List<SectionBlock> ParseSections(Block[] blocks, int minLevel = 2)
    {
        var root = new SectionBlock { Heading = null! }; // virtual root
        var stack = new Stack<SectionBlock>();
        stack.Push(root);

        foreach (var block in blocks)
        {
            if (block is HeadingBlock heading && heading.Level >= minLevel)
            {
                var section = new SectionBlock { Heading = heading };

                // Pop until we find a parent with lower level
                while (stack.Count > 1 && stack.Peek().Heading.Level >= heading.Level)
                {
                    stack.Pop();
                }

                stack.Peek().Children.Add(section);
                stack.Push(section);
            }
            else
            {
                // Add content to current top-level section
                if (stack.Peek() != root)
                {
                    stack.Peek().Content.Add(block);
                }
            }
        }

        return root.Children;
    }

    class SectionBlock
    {
        public HeadingBlock Heading { get; set; }
        public List<Block> Content { get; set; } = new();
        public List<SectionBlock> Children { get; set; } = new();
    }

    static string SanitizeId(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return "";

        // Remove quotes and any other problematic characters
        return id
            .Replace("\"", "")
            .Replace("'", "")
            .Replace("`", "")
            .Replace("”", "")
            .Replace("“", "")
            .Replace("’", "")
            .Replace("‘", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("-&-", "-")
            .Replace("&", "")
            .Replace("!", "")
            .Replace("?", "")
            .Trim();
    }
}