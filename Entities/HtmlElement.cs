using AngleSharp.Dom;
using Riok.Mapperly.Abstractions;

namespace ParsingHTML.Entities;


public class HtmlElement
{
    public string? Id { get; set; }
    public string? AttributeValue { get; set; }
    public string? HtmlValue { get; set; }
}