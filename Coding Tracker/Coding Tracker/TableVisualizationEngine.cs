using Spectre.Console;

namespace Coding_Tracker;

public class TableVisualizationEngine
{
    public void ShowTable(List<CodingSession> tableData)
    {
        var table = new Table();
        table.Title("Coding Sessions");
        table.AddColumn("Id");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");

        foreach (var session in tableData)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StartTime ?? "",
                session.EndTime ?? "",
                session.Duration ?? ""
            );
        }

        AnsiConsole.Write(table);
    }
}