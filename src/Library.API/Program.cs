using Library.API;
using Library.API.Infrastructure;
using Library.API.Middlewares;
using Library.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();
builder
  .Services
  .AddAuthentication("Bearer")
  .AddJwtBearer(opt =>
  {
    opt.TokenValidationParameters = JwtAuthProvider.GetTokenValidationParameters();
  });

builder
  .Services
  .AddSwaggerGen(options =>
  {
    var xmlFile = $"Library.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition(
      "Bearer",
      new OpenApiSecurityScheme
      {
        Description = @"Enter JWT Token please.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
      }
    );
    options.AddSecurityRequirement(
      new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
          },
          new List<string>()
        }
      }
    );
  });

builder
  .Services
  .AddAutoMapper(
    typeof(AuthorMappingProfile),
    typeof(GenreMappingProfile),
    typeof(BookInstanceMappingProfile),
    typeof(BookEditionMappingProfile)
  );

builder
  .Services
  .AddDbContext<LibraryContext>(
    options =>
      options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")
      )
  );

builder.Services.AddTransient<ILibraryService, LibraryService>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();

var app = builder.Build();

app.UseMiddleware<LibraryExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  var context = services.GetRequiredService<LibraryContext>();
  if (context.Database.GetPendingMigrations().Any())
  {
    context.Database.EnsureCreated();

    var seeder = new LibraryContextSeed();
    await seeder.SeedAsync(context);
  }
}

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = String.Empty;
  });
}

app.MapGroup("/api/v1/library").WithTags("Library API").MapLibraryApi();

app.Run();
