using Dapper;

namespace ParsingHTML;



public class DBInitializer
{
    public static async Task InitializeDatabase(string connString)
    {
        using var conn = new Npgsql.NpgsqlConnection(connString);
        conn.Open();

        var createTableQuery = @"
            CREATE TABLE IF NOT EXISTS elements (
                Id bigserial PRIMARY KEY,    
                AttributeValue TEXT,
                HtmlValue TEXT
            );
        ";

        var dropTableQuery = @"
            DROP TABLE IF EXISTS elements;
        ";

        await conn.ExecuteAsync(dropTableQuery);

        await conn.ExecuteAsync(createTableQuery);
    }
}