using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using ParsingHTML.Services.Abstractions;

namespace ParsingHTML.Services.Concrete;



public class HtmlElementService : IHtmlElementsService
{
    private readonly HtmlParser _parser = new();
    public IHtmlCollection<IElement> GetElementsByCssSelectorFromHtml(string selector, string htmlContent)
    {
        var doc = _parser.ParseDocument(htmlContent);
        var elements = doc.QuerySelectorAll(selector);
        return elements;
    }

    public IEnumerable<string> GetEmailsFromHtml(string htmlContent)
    {
        var matches = Regexs.FindEmail().Matches(htmlContent);

        var emails = matches.Select(e => e.Value);

        return emails;
    }
}

public partial class Regexs
{
    [GeneratedRegex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}")]
    public static partial Regex FindEmail();
}