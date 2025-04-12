namespace WebSql;

public class WebSqlResultset
{
    public required string[] Columns { get; set; }

    public required object[][] Values { get; set; }
}
