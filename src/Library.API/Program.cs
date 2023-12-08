using AutoMapper;
using Library.API;
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
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = String.Empty;
  });
}

app.MapGet("/", () => "hello");

app.MapGroup("/api/v1/library").WithTags("Library API").MapLibraryApi();

app.Run("http://localhost:3000");
