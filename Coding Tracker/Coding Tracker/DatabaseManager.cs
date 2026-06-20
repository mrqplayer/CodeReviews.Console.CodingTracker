using Dapper;
using Microsoft.Data.Sqlite;

namespace Coding_Tracker;

public class DatabaseManager
{
    public void CreateTable(string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Execute(
                @"CREATE TABLE IF NOT EXISTS coding_tracker (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration TEXT
                )");
        }
    }
}