using Library.API.Services;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();

try {
  var service = new LibraryService();
  var bookEdition = service.GetBookByISBN("978-5-04-111308-7");
  System.Console.WriteLine(bookEdition.Genres.Count());
}
catch(MySqlConnector.MySqlException e)
{
  logger.LogError(0, e, "Error while refer to database");
}

app.MapGet("/", () => "Hello World!");

app.Run();
