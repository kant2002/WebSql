using Microsoft.JSInterop;

namespace WebSql;

public class WebSqlStatement : IAsyncDisposable
{
    private IJSObjectReference objectReference;

    public WebSqlStatement(IJSObjectReference objectReference)
    {
        this.objectReference = objectReference;
    }

    /// <summary>
    /// Bind values to the parameters, after having reseted the statement. If values is null, do nothing and return true.
    /// </summary>
    /// <param name="parameterValues">Array of values for anonymous parameters specificed in statement SQL.</param>
    /// <returns>Returns <c>true</c> if binding was successful; <c>false</c> otherwise.</returns>
    public async ValueTask<bool> BindAsync(params object[] parameterValues)
    {
        return await objectReference.InvokeAsync<bool>("bind", parameterValues);
    }

    /// <summary>
    /// Bind values to the parameters, after having reseted the statement. If values is null, do nothing and return true.
    /// </summary>
    /// <param name="parameterValues">Dictionary with name of parameters as keys and values as parameter values.</param>
    /// <returns>Returns <c>true</c> if binding was successful; <c>false</c> otherwise.</returns>
    public async ValueTask<bool> BindAsync(Dictionary<string, object>? parameterValues)
    {
        return await objectReference.InvokeAsync<bool>("bind", parameterValues);
    }

    /// <summary>
    /// Gets names of the columns in the statement.
    /// </summary>
    /// <returns>An asynchronous task returning array of column names</returns>
    public async ValueTask<string[]> GetColumnNamesAsync()
    {
        return await objectReference.InvokeAsync<string[]>("getColumnNames");
    }

    /// <summary>
    /// Get the SQL string used in preparing this statement.
    /// </summary>
    /// <returns>An asynchronous task returning sql used in preparing this statement.</returns>
    public async ValueTask<string> GetSqlAsync()
    {
        return await objectReference.InvokeAsync<string>("getSQL");
    }

    /// <summary>
    /// Get the SQLite's normalized version of the SQL string used in preparing this statement.
    /// </summary>
    /// <returns>An asynchronous task returning normalized version of the sql string used in preparing this statement.</returns>
    /// <remarks>
    /// The meaning of "normalized" is not well-defined: see <see cref="https://sqlite.org/c3ref/expanded_sql.html"/>
    /// </remarks>
    public async ValueTask<string> GetNormalizedSqlAsync()
    {
        return await objectReference.InvokeAsync<string>("getNormalizedSQL");
    }

    /// <summary>
    /// Reset a statement, so that its parameters can be bound to new values.
    /// </summary>
    /// <returns>An task representing asynchronous operation.</returns>
    /// <remarks>
    /// It also clears all previous bindings, freeing the memory used by bound parameters.
    /// </remarks>
    public async ValueTask ResetAsync()
    {
        await objectReference.InvokeVoidAsync("reset");
    }

    /// <summary>
    /// Free the memory used by the statement.
    /// </summary>
    /// <returns>Returns <c>true</c> if freeing memory was successful; <c>false</c> otherwise.</returns>
    public async ValueTask<bool> FreeAsync()
    {
        return await objectReference.InvokeAsync<bool>("free");
    }

    /// <summary>
    /// Free the memory allocated during parameter binding
    /// </summary>
    /// <returns>An task representing asynchronous operation.</returns>
    public async ValueTask FreeParameterMemoryAsync()
    {
        await objectReference.InvokeVoidAsync("freemem");
    }

    /// <summary>
    /// Get one row of results of a statement.
    /// </summary>
    /// <param name="parameterValues">Array of values for anonymous parameters specificed in statement SQL.</param>
    /// <returns>Array of row values.</returns>
    /// <remarks>If the first parameter is not provided, step must have been called before.</remarks>
    public async ValueTask<object[]> GetAsync(params object[] parameterValues)
    {
        return await objectReference.InvokeAsync<object[]>("get", parameterValues);
    }

    /// <summary>
    /// Get one row of results of a statement.
    /// </summary>
    /// <param name="parameterValues">Dictionary with name of parameters as keys and values as parameter values.</param>
    /// <returns>Array of row values.</returns>
    /// <remarks>If the first parameter is not provided, step must have been called before.</remarks>
    public async ValueTask<object[]> GetAsync(Dictionary<string, object>? parameterValues)
    {
        return await objectReference.InvokeAsync<object[]>("get", parameterValues);
    }

    /// <summary>
    /// Get one row of result as a javascript object, associating column names with their value in the current row.
    /// </summary>
    /// <param name="parameterValues">Array of values for anonymous parameters specificed in statement SQL.</param>
    /// <returns>Single row of result as dictionary where key is column name and value is value of the column</returns>
    public async ValueTask<Dictionary<string, object>> GetAsObjectAsync(params object[] parameterValues)
    {
        return await objectReference.InvokeAsync<Dictionary<string, object>>("getAsObject", parameterValues);
    }

    /// <summary>
    /// Get one row of result as a javascript object, associating column names with their value in the current row.
    /// </summary>
    /// <param name="parameterValues">Dictionary with name of parameters as keys and values as parameter values.</param>
    /// <returns>Single row of result as dictionary where key is column name and value is value of the column</returns>
    public async ValueTask<Dictionary<string, object>> GetAsObjectAsync(Dictionary<string, object>? parameterValues)
    {
        return await objectReference.InvokeAsync<Dictionary<string, object>>("getAsObject", parameterValues);
    }

    /// <summary>
    /// Execute the statement, fetching the the next line of result, that can be retrieved with <see cref="GetAsync(object[])">.
    /// </summary>
    /// <returns>Returns <c>true</c> if a row of result available; <c>false</c> otherwise.</returns>
    public async ValueTask<bool> StepAsync()
    {
        return await objectReference.InvokeAsync<bool>("step");
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await FreeAsync();
        await objectReference.DisposeAsync();
    }
}
