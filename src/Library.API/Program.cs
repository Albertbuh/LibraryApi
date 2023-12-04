using Library.API.Services;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();

try {
  var service = new LibraryService();
}
catch(MySqlConnector.MySqlException e)
{
  logger.LogError(0, e, "Error while refer to database");
}

app.MapGet("/", () => "Hello World!");

app.Run();
