using AngleSharp.Dom;

namespace ParsingHTML.Services.Abstractions;


public interface IHtmlElementsService
{
    IHtmlCollection<IElement> GetElementsByCssSelectorFromHtml(string selector, string htmlContent);
    IEnumerable<string> GetEmailsFromHtml(string htmlContent);
}