# WebSql

The in-browser SQL engine powered by excellent <img src="https://user-images.githubusercontent.com/552629/76405509-87025300-6388-11ea-86c9-af882abb00bd.png" width="20px" alt="sql.js" /> [sql.js](https://sql.js.org/) 

![GitHub](https://img.shields.io/github/license/kant2002/websql?style=flat-square)

## Installation

```
dotnet add package WebSql --version 0.0.1
```

## Usage

Creaton of new database.
```csharp
var database = await WebSqlInterop.CreateDatabase();
```

Then you can execute SQL inside. Create schema, populate data
```csharp
string CreateDatabaseSql = @"
CREATE TABLE IF NOT EXISTS WeatherForecast (
	Id INTEGER PRIMARY KEY AUTOINCREMENT, 
	Date TEXT, 
	TemperatureC INTEGER, 
	Summary TEXT
);

INSERT INTO WeatherForecast(Date, TemperatureC, Summary)
VALUES ('2022-01-06', 1, 'Feezing'),
    ('2022-01-07', 14, 'Bracing'),
    ('2022-01-08', -13, 'Feezing'),
    ('2022-01-09', -16, 'Balmy'),
    ('2022-01-10', -2, 'Chilly'),
    ('2022-01-11', -3, 'Chilly');
";

await database.ExecuteNonQueryAsync(CreateDatabaseSql);
```

If you want to get results out of the database, you can use `ExecuteQueryAsync` method.
```csharp
WebSqlResultset[] result = await database.ExecuteAsync("SELECT * FROM WeatherForecast");
var firstResultset = result[0];
foreach (var row in result.Values)
{
    // Access the values in the row using column indexes
    Console.WriteLine($"Id: {row[0]}, Date: {row[1]}, TemperatureC: {row[2]}, Summary: {row[3]}");
}
```
Data in the row is stored as type `System.Text.Json.JsonElement`, so you should perform conversion yourself for now. No marshalling done on my side.

### Using parameters

You can use dictionaries to specify parameters. Sqlite supports `?`, `?NNN`, `:VVV`, `@VVV` and `$VVV` as parameter names, where `NNN` is a number and `VVV` a string.
You can use any of them in your SQL statement, but you have to use the same name in the dictionary.

```
string ParametersSampleSql = @"
DROP TABLE IF EXISTS test;

CREATE TABLE test (id INTEGER, age INTEGER, name TEXT);
INSERT INTO test VALUES ($id1, :age1, @name1);
INSERT INTO test VALUES ($id2, :age2, @name2);

SELECT id FROM test;
SELECT age,name FROM test WHERE id=$id1";

result = await database.ExecuteAsync(ParametersSampleSql, new Dictionary<string, object>()
        {
            {"$id1", 1 },
            { ":age1", 10 },
            { "@name1", "Ling" },
            { "$id2", 2 },
            { ":age2", 18 },
            { "@name2", "Paul"  }
        });
```

### Using cursor

If you don't want load whole dataset into memory, you can use `PrepareAsync` method.
It will create prepared `WebSqlStatement` class which can be used to bind parameters and step through the result set.

```csharp
string AnonymousParameterPrepareSql = "SELECT * FROM hello WHERE a=? AND b=?";

await using WebSqlStatement anonymousParameterStatement = await database.PrepareAsync(AnonymousParameterPrepareSql);
await anonymousParameterStatement.BindAsync(0, "hello");
while (await anonymousParameterStatement.StepAsync())
{
    var rowResult = await anonymousParameterStatement.GetAsync();
    // Do something with results
}
```

### Remote database

You can load database from remote location. Use `CreateDatabaseFromFile` method and specify URL.

```
WebSqlDatabase database = await WebSqlInterop.CreateDatabaseFromFile("Chinook_Sqlite.sqlite");
WebSqlResultset[] result = await database.ExecuteAsync("SELECT * FROM Artist LIMIT 10;");
```