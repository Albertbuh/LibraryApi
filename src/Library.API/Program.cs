using Library.API.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();

try {
  using(LibraryContext db = new LibraryContext())
  {
    var books = db.Genres.ToList();
    foreach (var item in books)
    {
      logger.LogInformation($"Genre: {item.Name}");
    }

    var authors = db.Authors.ToList();
    foreach(var item in authors)
    {
      logger.LogInformation($"Author: {item.ToString()}");
    }
  }
}
catch(MySqlConnector.MySqlException e)
{
  logger.LogError(0, e, "Error while refer to database");
}

app.MapGet("/", () => "Hello World!");

app.Run();
