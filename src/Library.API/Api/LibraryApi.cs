using AutoMapper;
using Library.API.Services;
using Library.API.Services.Exceptions;

namespace Library.API;

public static class LibraryAPI
{
  public static IEndpointRouteBuilder MapLibraryApi(this IEndpointRouteBuilder app)
  {
    app.MapGet("/items", GetAllBooks);
    app.MapGet("/items/by/{isbn}", GetBookByISBN);
    app.MapGet("/items/{id:int}", GetBookInstanceById);

    app.MapPost("/items", AddBookEdition);
    return app; 
  }

  private static IResult GetAllBooks(IMapper mapper, ILibraryService service)
  {
    var bookInstances = service.GetAllBooks();
    var dto = mapper.Map<List<BookInstanceDTO>>(bookInstances);

    return TypedResults.Json(dto);
  }

  private static async Task<IResult> GetBookInstanceById(IMapper mapper, ILibraryService service, int id)
  {
    var bookInstance = await service.GetBookInstanceById(id);
    
    if(bookInstance == null)
      return TypedResults.NotFound($"Not found instance with id {id}");

    var dto = mapper.Map<BookInstanceDTO>(bookInstance);

    return TypedResults.Json(dto);
  }

  private static async Task<IResult> GetBookByISBN(IMapper mapper, ILibraryService service, string isbn)
  {
    var bookEdition = await service.GetBookByISBN(isbn);

    if(bookEdition == null)
      return TypedResults.NotFound($"Not found edition with isbn -> {isbn}");

    var dto = mapper.Map<BookEditionDTO>(bookEdition);
    
    return TypedResults.Json(dto);
  }

  private static async Task<IResult> AddBookEdition(IMapper mapper, ILibraryService service, BookEditionDTO editionDTO)
  {
    IResult result = TypedResults.Ok();
    try
    {
      var edition = mapper.Map<BookEdition>(editionDTO);
      var response = await service.AddBookEdition(edition);
      
      if(!response.Result)
        result = TypedResults.BadRequest(response.Message); 
      else
        result = TypedResults.Created($"api/v1/library/items/by/{edition.ISBN}");

    }
    catch(LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message); 
    }
    
    return result;
  }
}
