using Microsoft.JSInterop;

namespace WebSql;

public class WebSqlDatabase : IAsyncDisposable
{
    internal readonly IJSObjectReference objectReference;

    public WebSqlDatabase(IJSObjectReference objectReference)
    {
        this.objectReference = objectReference;
    }

    public async ValueTask ExecuteNonQueryAsync(string sql)
    {
        await objectReference.InvokeVoidAsync("run", sql);
    }

    public async ValueTask<WebSqlResultset[]> ExecuteAsync(string sql)
    {
        return await objectReference.InvokeAsync<WebSqlResultset[]>("exec", sql);
    }

    public async ValueTask DisposeAsync()
    {
        await objectReference.DisposeAsync();
    }
}
