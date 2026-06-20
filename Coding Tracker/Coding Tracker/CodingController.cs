using Dapper;
using Microsoft.Data.Sqlite;

namespace Coding_Tracker;

public class CodingController
{
    private readonly string _connectionString;

    public CodingController(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Add(CodingSession session)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute(
                "INSERT INTO coding_tracker (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)",
                session);
        }
    }

    public List<CodingSession> Get()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            var sql = "SELECT * FROM coding_tracker";
            var sessions = connection.Query<CodingSession>(sql);

            return sessions.ToList();
        }
    }

    public void Delete(int id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute("DELETE FROM coding_tracker WHERE Id = @Id", new { Id = id });
        }
    }

    public void Update(CodingSession session)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Execute(
                @"UPDATE coding_tracker
              SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration
              WHERE Id = @Id", session);
        }
    }

    public List<CodingSession> GetFiltered(int days)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            var sessions = connection.Query<CodingSession>("SELECT * FROM coding_tracker").ToList();

            return sessions
                .Where(s => DateTime.ParseExact(s.StartTime ?? "", "dd-MM-yyyy HH:mm", null) >= DateTime.Now.AddDays(-days))
                .ToList();
        }
    }

    public List<CodingSession> GetAllSorted(bool ascending)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            var sessions = connection.Query<CodingSession>("SELECT * FROM coding_tracker").ToList();

            return ascending
                ? sessions.OrderBy(s => DateTime.ParseExact(s.StartTime ?? "", "dd-MM-yyyy HH:mm", null)).ToList()
                : sessions.OrderByDescending(s => DateTime.ParseExact(s.StartTime ?? "", "dd-MM-yyyy HH:mm", null)).ToList();
        }
    }

    public List<CodingSession> GetFilteredAndSorted(int days, bool ascending)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            var sessions = connection.Query<CodingSession>("SELECT * FROM coding_tracker").ToList();

            if (days > 0)
            {
                sessions = sessions
                    .Where(s => DateTime.ParseExact(s.StartTime ?? "", "dd-MM-yyyy HH:mm", null) >= DateTime.Now.AddDays(-days))
                    .ToList();
            }

            return ascending
                ? sessions.OrderBy(s => DateTime.ParseExact(s.StartTime ?? "", "dd-MM-yyyy HH:mm", null)).ToList()
                : sessions.OrderByDescending(s => DateTime.ParseExact(s.StartTime ?? "", "dd-MM-yyyy HH:mm", null)).ToList();
        }
    }
}