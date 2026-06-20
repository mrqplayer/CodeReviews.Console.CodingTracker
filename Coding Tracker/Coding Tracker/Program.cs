using Coding_Tracker;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

string? connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";

var dbManager = new DatabaseManager();
dbManager.CreateTable(connectionString);

var codingController = new CodingController(connectionString);
var userInput = new UserInput(codingController);

userInput.Menu();