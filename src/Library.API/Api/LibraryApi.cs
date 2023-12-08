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
    app.MapPost("/items", AddBookEdition).RequireAuthorization();
    app.MapPut("/items/by/{isbn}", UpdateBookEdition).RequireAuthorization();
    app.MapDelete("/items/by/{isbn}", DeleteBookEdition).RequireAuthorization();

    app.MapGet("/items/{id:int}", GetBookInstanceById);
    app.MapPost("/items/by/{isbn}", AddBookInstances).RequireAuthorization();
    app.MapPut("/items/{id:int}", UpdateBookInstance).RequireAuthorization();
    app.MapDelete("/items/{id:int}", DeleteBookInstance).RequireAuthorization();

    return app;
  }

  ///<summary>
  /// Get all book instances 
  /// </summary>
  private static IResult GetAllBooks(IMapper mapper, ILibraryService service)
  {
    IResult result = TypedResults.Ok();
    try
    {
      var bookInstances = service.GetAllBooks();
      var dto = mapper.Map<List<BookInstanceDTO>>(bookInstances);
      result = TypedResults.Json(dto);
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }
    return result;
  }

  
  ///<summary>
  /// Get book instance using Id
  /// </summary>
  private static async Task<IResult> GetBookInstanceById(
    IMapper mapper,
    ILibraryService service,
    int id
  )
  {
    IResult result = TypedResults.Ok();
    try
    {
      var bookInstance = await service.GetBookInstanceById(id);

      if (bookInstance == null)
        result = TypedResults.NotFound($"Not found instance with id {id}");
      else
      {
        var dto = mapper.Map<BookInstanceDTO>(bookInstance);
        result = TypedResults.Json(dto);
      }
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }

    return result;
  }

  ///<summary>
  /// Get book edition using ISBN
  /// </summary>
  private static async Task<IResult> GetBookByISBN(
    IMapper mapper,
    ILibraryService service,
    string isbn
  )
  {
    IResult result = TypedResults.Ok();
    try
    {
      var bookEdition = await service.GetBookByISBN(isbn);

      if (bookEdition == null)
        result = TypedResults.NotFound($"Not found edition with isbn -> {isbn}");
      else
      {
        var dto = mapper.Map<BookEditionDTO>(bookEdition);
        result = TypedResults.Json(dto);
      }
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }

    return result;
  }

  ///<summary>
  /// Create new book edition
  /// </summary>
  private static async Task<IResult> AddBookEdition(
    IMapper mapper,
    ILibraryService service,
    BookEditionDTO editionDTO
  )
  {
    IResult result = TypedResults.Ok();
    try
    {
      var edition = mapper.Map<BookEdition>(editionDTO);
      var response = await service.AddBookEdition(edition);

      if (!response.Result)
        result = TypedResults.BadRequest(response.Message);
      else
        result = TypedResults.Created($"api/v1/library/items/by/{edition.ISBN}");
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }

    return result;
  }

  ///<summary>
  /// Create new book instances
  /// </summary>
  /// <param name="isbn">ISBN code of book edition, which instances need to add</param>
  /// <param name="amount">amount of books</param>
  private static async Task<IResult> AddBookInstances(
    ILibraryService service,
    string isbn,
    int amount
  )
  {
    IResult result = TypedResults.Ok();
    try
    {
      var response = await service.AddBookInstances(isbn, amount);
      if (response.Result == false)
        result = TypedResults.BadRequest(response.Message);
      else
        result = TypedResults.Ok(response.Message);
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }
    return result;
  }

  ///<summary>
  /// Update book edition info
  /// </summary>
  private static async Task<IResult> UpdateBookEdition(
    IMapper mapper,
    ILibraryService service,
    string isbn,
    BookEditionDTO newBookEditionInfo
  )
  {
    IResult result = TypedResults.Ok();
    try
    {
      var info = mapper.Map<BookEdition>(newBookEditionInfo);
      var response = await service.UpdateBookEdition(isbn, info);

      if (response.Result)
        result = TypedResults.Ok(response.Message);
      else
        result = TypedResults.BadRequest(response.Message);
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }

    return result;
  }

  ///<summary>
  /// Update book instance data
  /// </summary>
  /// <remarks>
  /// The only data of book instance which is changing is date of taking and returning 
  /// </remarks>
  private static async Task<IResult> UpdateBookInstance(
    IMapper mapper,
    ILibraryService service,
    int id,
    BookInstanceDTO newBookInstance
  )
  {
    IResult result = TypedResults.Ok();
    try
    {
      var info = mapper.Map<BookInstance>(newBookInstance);
      var response = await service.UpdateBookInstance(id, info);

      if (response.Result)
        result = TypedResults.Ok(response.Message);
      else
        result = TypedResults.BadRequest(response.Message);
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }

    return result;
  }

  ///<summary>
  /// Delete book edition
  /// </summary>
  private static async Task<IResult> DeleteBookEdition(ILibraryService service, string isbn)
  {
    IResult result = TypedResults.Ok();

    try
    {
      var response = await service.DeleteBookEdition(isbn);

      if (response.Result)
        result = TypedResults.NoContent();
      else
        result = TypedResults.NotFound(response.Message);
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }

    return result;
  }

  ///<summary>
  /// Delete book instance
  /// </summary>
  private static async Task<IResult> DeleteBookInstance(ILibraryService service, int id)
  {
    IResult result = TypedResults.Ok();

    try
    {
      var response = await service.DeleteBookInstance(id);

      if (response.Result)
        result = TypedResults.NoContent();
      else
        result = TypedResults.NotFound(response.Message);
    }
    catch (LibraryServiceException e)
    {
      result = TypedResults.BadRequest(e.Message);
    }

    return result;
  }
}
