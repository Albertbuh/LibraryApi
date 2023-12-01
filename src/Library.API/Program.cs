using Library.API.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();

try {
  using(LibraryContext db = new LibraryContext())
  {
    var books = db.Books.ToList();
    logger.LogInformation($"Num of books in DB: {books.Count()}");
  }
}
catch(MySqlConnector.MySqlException e)
{
  logger.LogError(0, e, "Error while refer to database");
}

app.MapGet("/", () => "Hello World!");

app.Run();
