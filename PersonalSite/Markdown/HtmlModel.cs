using Ganss.XSS;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Microsoft.AspNetCore.Components;

namespace PersonalSite.Markdown
{
    public partial class HtmlModel : ComponentBase
    {
        private string _content;

        [Inject] public IHtmlSanitizer HtmlSanitizer { get; set; } = null!;

        [Parameter]
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                HtmlContent = ConvertStringToMarkupString(_content);
            }
        }

        [Parameter]
        public bool Truncate { get; set; }

        public MarkupString HtmlContent { get; private set; }

        private MarkupString ConvertStringToMarkupString(string value)
        {
            if (!string.IsNullOrWhiteSpace(_content))
            {
                // Sanitize HTML before rendering
                var sanitizedHtml = HtmlSanitizer.Sanitize(value);

                if (Truncate)
                {
                    sanitizedHtml = sanitizedHtml.TruncateHtml(500);
                }

                // Return sanitized HTML as a MarkupString that Blazor can render
                return new MarkupString(sanitizedHtml);
            }

            return new MarkupString();
        }
    }
}