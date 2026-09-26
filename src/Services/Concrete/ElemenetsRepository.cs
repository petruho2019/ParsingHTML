using System.Data;
using Dapper;
using Npgsql;
using ParsingHTML.Entities;
using ParsingHTML.Services.Abstractions;

namespace ParsingHTML.Services.Concrete;

public class ElementsRepository : IElementsRepository, IDisposable
{
    private readonly IDbConnection _conn;
    public ElementsRepository(string connString)
    {
        _conn = new NpgsqlConnection(connString);
        _conn.Open();
    }

    private static readonly string _insertQuery = $@"insert into elements
        (AttributeValue, HtmlValue)
        values
        (@{nameof(HtmlElement.AttributeValue)},
        @{nameof(HtmlElement.HtmlValue)})";

    public async Task<int> AddElementsAsync(IEnumerable<HtmlElement> elements)
    {
        return await _conn.ExecuteAsync(_insertQuery, elements);
    }

    public void Dispose()
    {
        _conn.Dispose();
    }
}