using AutoMapper;
using Library.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
  .Services
  .AddAutoMapper(
    typeof(AuthorMappingProfile),
    typeof(GenreMappingProfile),
    typeof(BookInstanceMappingProfile),
    typeof(BookEditionMappingProfile)
  );
  
builder.Services.AddTransient<ILibraryService, LibraryService>();

ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  logger.LogInformation("Swagger connected");
}

app.MapGet("/", () => "hello");
app.MapGet(
  "/catalog",
  (IMapper mapper, ILibraryService service) =>
  {
    var bookInstances = service.GetAllBooks();
    var dto = mapper.Map<List<Library.API.Models.DTO.BookInstanceDTO>>(bookInstances);

    return TypedResults.Json(dto);
  }
);

app.Run();
