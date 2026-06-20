namespace Coding_Tracker;

public class UserInput
{
    private readonly CodingController _controller;

    public UserInput(CodingController controller)
    {
        _controller = controller;
    }

    public void GetNewSessionInput()
    {
        string? startTime = GetDateInput("Please input start time (format: dd-MM-yyyy HH-MM) or type 0 to return to main menu.");
        if (startTime == "0") return;

        string? endTime = GetDateInput("Please input end time (format: dd-MM-yyyy HH-MM) or type 0 to return to main menu.");
        if (endTime == "0") return;

        while (!Validation.IsEndDateValid(startTime, endTime))
        {
            Console.WriteLine("End Time cannot be earlier than start time.");
            endTime = GetDateInput("Please insert a valid end time");
        }

        var duration = CalculateDuration(startTime, endTime);
        var session = new CodingSession { StartTime = startTime, EndTime = endTime, Duration = duration };
        _controller.Add(session);

        Console.WriteLine($"Session saved! Duration: {duration}");
    }

    private string GetDateInput(string message)
    {
        Console.WriteLine(message + " Or type 't' for today");
        string input = Console.ReadLine()?.ToLower() ?? "";

        if (input == "t")
        {
            return DateTime.Now.ToString("dd-MM-yyyy HH:mm");
        }

        while (input != "0" && !Validation.IsValidDate(input))
        {
            Console.WriteLine("Invalid format. Please try again (dd-MM-yyyy HH:mm)");
            input = Console.ReadLine() ?? "";
        }
        return input;
    }

    private string CalculateDuration(string startTime, string endTime)
    {
        DateTime start = DateTime.ParseExact(startTime, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        DateTime end = DateTime.ParseExact(endTime, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

        TimeSpan duration = end - start;

        return $"{(int)duration.TotalHours}h {duration.Minutes}m";
    }

    public void Menu()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("----------------------------");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Type 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Add Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("Type 5 to Start Stopwatch Session.");
            Console.WriteLine("Type 6 to Filter or Sort Records.");
            Console.WriteLine("----------------------------\n");

            string? command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    break;

                case "1":
                    ViewRecords();
                    break;

                case "2":
                    GetNewSessionInput();
                    break;

                case "3":
                    ProcessDelete();
                    break;

                case "4":
                    ProcessUpdate();
                    break;

                case "5":
                    ProcessStopwatch();
                    break;

                case "6":
                    ProcessFilterAndSort();
                    break;

                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 6.\n");
                    break;
            }
        }
    }

    private void ViewRecords()
    {
        var sessions = _controller.Get();
        var tableVisualisation = new TableVisualizationEngine();
        tableVisualisation.ShowTable(sessions);
    }

    private void ProcessDelete()
    {
        Console.WriteLine("\nPlease type the Id of the record you want to delete (or 0 to return to main menu):");
        string? input = Console.ReadLine();

        if (input == "0") return;

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("\nInvalid input. Please enter a numeric Id.");
            return;
        }

        _controller.Delete(id);
        Console.WriteLine($"Record {id} has been deleted");
    }

    private void ProcessUpdate()
    {
        Console.WriteLine("\nPlease type the Id of the record you want to update (or 0 to return to main menu):");
        string? input = Console.ReadLine();

        if (input == "0" || !int.TryParse(input, out int id))
        {
            Console.WriteLine("\n Invalid input");
            return;
        }

        string startTime = GetDateInput("\nPlease insert the new start time (format: dd-MM-yyyy HH:mm):");
        string endTime = GetDateInput("Please insert the new end time (format: dd-MM-yyyy HH:mm):");

        while (!Validation.IsEndDateValid(startTime, endTime))
        {
            Console.WriteLine("Error: End time cannot be earlier than start time.");
            endTime = GetDateInput("Please insert a valid end time:");
        }

        var duration = CalculateDuration(startTime, endTime);
        var updatedSession = new CodingSession
        {
            Id = id,
            StartTime = startTime,
            EndTime = endTime,
            Duration = duration
        };

        _controller.Update(updatedSession);
        Console.WriteLine("\nRecord updated successfully!");
    }

    private void ProcessStopwatch()
    {
        Console.WriteLine("\nStopwatch started! Press any key to stop the timer...");
        DateTime startDateTime = DateTime.Now;

        Console.ReadKey();

        DateTime endDateTime = DateTime.Now;
        string startTime = startDateTime.ToString("dd-MM-yyyy HH:mm");
        string endTime = endDateTime.ToString("dd-MM-yyyy HH:mm");

        var duration = CalculateDuration(startTime, endTime);
        var session = new CodingSession
        {
            StartTime = startTime,
            EndTime = endTime,
            Duration = duration
        };

        _controller.Add(session);
        Console.WriteLine($"\nStopwatch stopped! Session saved with duration: {duration}");
    }

    private void ProcessFilterAndSort()
    {
        Console.WriteLine("\nFilter by period:");
        Console.WriteLine("1 - Last 24 hours");
        Console.WriteLine("2 - Last 7 days");
        Console.WriteLine("3 - This year");
        Console.WriteLine("0 - All time / Skip");
        string? filterChoice = Console.ReadLine();

        int days = filterChoice switch
        {
            "1" => 1,
            "2" => 7,
            "3" => 365,
            _ => 0
        };

        Console.WriteLine("\nSort order:");
        Console.WriteLine("1 - Ascending");
        Console.WriteLine("2 - Descending");
        string? sortChoice = Console.ReadLine();

        bool ascending = sortChoice != "2";

        var sessions = _controller.GetFilteredAndSorted(days, ascending);
        new TableVisualizationEngine().ShowTable(sessions);
    }
}