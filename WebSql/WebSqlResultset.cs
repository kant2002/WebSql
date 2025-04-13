using System.Runtime.InteropServices.JavaScript;

namespace WebSql;

/// <summary>
/// WebSqlResultset class represents the result set of a SQL query executed against a WebSql database.
/// </summary>
public class WebSqlResultset
{
    /// <summary>
    /// Gets or sets the name of columns in the result set.
    /// </summary>
    public required string[] Columns { get; set; }

    /// <summary>
    /// Gets or sets the rows data n the result set.
    /// </summary>
    public required JSObject[][] Values { get; set; }
}
