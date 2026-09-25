using ParsingHTML.Entities;

namespace ParsingHTML.Services.Abstractions;



public interface IElementsRepository
{
    Task<int> AddElementsAsync(IEnumerable<HtmlElement> elements);
}