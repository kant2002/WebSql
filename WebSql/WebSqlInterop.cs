using Microsoft.JSInterop;

namespace WebSql;

public class WebSqlInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public WebSqlInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/WebSql/sql-launcher.js").AsTask());
    }

    /// <summary>
    /// Creates a new empty Sqlite database asynchronously.
    /// </summary>
    /// <returns>Returns a ValueTask containing a WebSqlDatabase instance.</returns>
    public async ValueTask<WebSqlDatabase> CreateDatabase()
    {
        var module = await moduleTask.Value;
        return new(await module.InvokeAsync<IJSObjectReference>("createDatabase"));
    }

    /// <summary>
    /// Creates a WebSqlDatabase instance from a specified SQLite remote location asynchronously.
    /// </summary>
    /// <param name="sqliteFileLocation">Specifies the remote location of the SQLite file to be used for creating the database.</param>
    /// <returns>Returns a WebSqlDatabase object created from the provided SQLite file.</returns>
    public async ValueTask<WebSqlDatabase> CreateDatabaseFromFile(string sqliteFileLocation)
    {
        var module = await moduleTask.Value;
        return new(await module.InvokeAsync<IJSObjectReference>("createDatabaseFromFile", sqliteFileLocation));
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
