namespace PersonalSite.Markdown;

using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Renderers.Html;

public class CustomTableRenderer : HtmlObjectRenderer<Table>
{
    protected override void Write(HtmlRenderer renderer, Table table)
    {
        renderer
            .Write("<table class=\"table\">")
            .WriteAttributes(table)
            .WriteLine(">");
        renderer.WriteChildren(table);
        renderer.WriteLine("</table>");
    }
}