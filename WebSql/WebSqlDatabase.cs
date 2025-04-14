using Microsoft.JSInterop;

namespace WebSql;

/// <summary>
/// Reference to Sql.js database instance in the JavaScript context.
/// </summary>
public class WebSqlDatabase : IAsyncDisposable
{
    internal readonly IJSObjectReference objectReference;

    public WebSqlDatabase(IJSObjectReference objectReference)
    {
        this.objectReference = objectReference;
    }

    /// <summary>
    /// Execute an SQL query, ignoring the rows it returns.
    /// </summary>
    /// <param name="sql">A string containing some SQL text to execute.</param>
    /// <param name="parameterValues">Array of values for anonymous parameters specificed in statement SQL.</param>
    /// <returns>An task representing asynchronous operation.</returns>
    public async ValueTask ExecuteNonQueryAsync(string sql, params object[] parameterValues)
    {
        await objectReference.InvokeVoidAsync("run", sql, parameterValues);
    }

    /// <summary>
    /// Execute an SQL query, ignoring the rows it returns.
    /// </summary>
    /// <param name="sql">A string containing some SQL text to execute.</param>
    /// <param name="parameterValues">Dictionary with name of parameters as keys and values as parameter values.</param>
    /// <returns>An task representing asynchronous operation.</returns>
    public async ValueTask ExecuteNonQueryAsync(string sql, Dictionary<string, object>? parameterValues)
    {
        await objectReference.InvokeVoidAsync("run", sql, parameterValues);
    }

    /// <summary>
    /// Execute an SQL query, and returns the result.
    /// </summary>
    /// <param name="sql">A string containing some SQL text to execute.</param>
    /// <param name="parameterValues">Array of values for anonymous parameters specificed in statement SQL.</param>
    /// <returns>Results of each passed as <see cref="WebSqlResultset"/>.</returns>
    /// <remarks>
    /// If you use the params argument as an array, you cannot provide an sql string that contains several statements (separated by ;). This limitation does not apply to params as an object.
    /// </remarks>
    public async ValueTask<WebSqlResultset[]> ExecuteAsync(string sql, params object[] parameterValues)
    {
        return await objectReference.InvokeAsync<WebSqlResultset[]>("exec", sql, parameterValues);
    }

    /// <summary>
    /// Execute an SQL query, and returns the result.
    /// </summary>
    /// <param name="sql">A string containing some SQL text to execute.</param>
    /// <param name="parameterValues">Dictionary with name of parameters as keys and values as parameter values.</param>
    /// <returns>Results of each passed as <see cref="WebSqlResultset"/>.</returns>
    /// <remarks>
    /// If you use the params argument as an array, you cannot provide an sql string that contains several statements (separated by ;). This limitation does not apply to params as an object.
    /// </remarks>
    public async ValueTask<WebSqlResultset[]> ExecuteAsync(string sql, Dictionary<string, object>? parameterValues)
    {
        return await objectReference.InvokeAsync<WebSqlResultset[]>("exec", sql, parameterValues);
    }

    /// <summary>
    /// Prepare an SQL statement.
    /// </summary>
    /// <param name="sql">A string containing some SQL text to prepare.</param>
    /// <param name="parameterValues">Array of values for anonymous parameters specificed in statement SQL to bind to parameters.</param>
    /// <returns>A <see cref="WebSqlStatement"/> representing prepared SQL statement.</returns>
    public async ValueTask<WebSqlStatement> PrepareAsync(string sql, params object[] parameterValues)
    {
        var statementReference = await objectReference.InvokeAsync<IJSObjectReference>("prepare", sql, parameterValues);
        return new WebSqlStatement(statementReference);
    }

    /// <summary>
    /// Prepare an SQL statement.
    /// </summary>
    /// <param name="sql">A string containing some SQL text to prepare.</param>
    /// <param name="parameterValues">Dictionary with name of parameters as keys and values as parameter values.</param>
    /// <returns>A <see cref="WebSqlStatement"/> representing prepared SQL statement.</returns>
    public async ValueTask<WebSqlStatement> PrepareAsync(string sql, Dictionary<string, object>? parameterValues)
    {
        var statementReference = await objectReference.InvokeAsync<IJSObjectReference>("prepare", sql, parameterValues);
        return new WebSqlStatement(statementReference);
    }

    /// <summary>
    /// Returns the number of changed rows (modified, inserted or deleted) by the latest completed INSERT, UPDATE or DELETE statement on the database. 
    /// </summary>
    /// <returns>The number of changed rows (modified, inserted or deleted) by the latest completed INSERT, UPDATE or DELETE statement on the database</returns>
    /// <remarks>
    /// Executing any other type of SQL statement does not modify the value returned by this function.
    /// </remarks>
    public async ValueTask<int> GetRowsModifiedAsync()
    {
        return await objectReference.InvokeAsync<int>("getRowsModified");
    }

    /// <summary>
    /// Analyze a result code, return null if no error occured, and throw an error with a descriptive message otherwise
    /// </summary>
    /// <returns>An task representing asynchronous operation.</returns>
    public async ValueTask HandleErrorAsync()
    {
        await objectReference.InvokeVoidAsync("handleError");
    }

    public async ValueTask DisposeAsync()
    {
        await objectReference.InvokeVoidAsync("close");
        await objectReference.DisposeAsync();
    }
}
