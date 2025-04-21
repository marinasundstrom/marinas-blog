namespace PersonalSite.Markdown;

using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

public class NoIdHeadingRenderer : HtmlObjectRenderer<HeadingBlock>
{
    protected override void Write(HtmlRenderer renderer, HeadingBlock obj)
    {
        //Console.WriteLine("Rendering heading: " + obj.Inline?.FirstChild?.ToString());

        renderer.EnsureLine();
        renderer.Write($"<h{obj.Level}>");

        renderer.WriteLeafInline(obj);

        renderer.WriteLine($"</h{obj.Level}>");
    }
}
