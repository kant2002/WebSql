using Microsoft.JSInterop;

namespace WebSql;

public class WebSqlInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public WebSqlInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/WebSql/sql-launcher.js").AsTask());
    }

    public async ValueTask<WebSqlDatabase> CreateDatabase()
    {
        var module = await moduleTask.Value;
        return new(await module.InvokeAsync<IJSObjectReference>("createDatabase"));
    }

    public async ValueTask<WebSqlDatabase> CreateDatabaseFromFile(string sqliteFileLocation)
    {
        var module = await moduleTask.Value;
        return new(await module.InvokeAsync<IJSObjectReference>("createDatabaseFromFile", sqliteFileLocation));
    }

    public async ValueTask Exec(WebSqlDatabase db, string sql)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("exec", db.objectReference, sql);
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
