using Library.API.Services;

namespace Library.API;

public static class LibraryAPI
{
  public static IEndpointRouteBuilder MapLibraryApi(this IEndpointRouteBuilder app)
  {
    app.MapGet("/items", GetAllBookInstances);
    app.MapGet("/items/by", GetAllBooks);

    //work with editions
    app.MapGet("/items/by/{isbn}", GetBookByISBN);
    app.MapPost("/items", AddBookEdition).RequireAuthorization();
    app.MapPut("/items/by/{isbn}", UpdateBookEdition).RequireAuthorization();
    app.MapDelete("/items/by/{isbn}", DeleteBookEdition).RequireAuthorization();

    //work with instances
    app.MapGet("/items/{id:int}", GetBookInstanceById);
    app.MapPost("/items/by/{isbn}", AddBookInstances).RequireAuthorization();
    app.MapPut("/items/{id:int}", UpdateBookInstance).RequireAuthorization();
    app.MapDelete("/items/{id:int}", DeleteBookInstance).RequireAuthorization();

    app.MapGet("/token", GetToken);
    return app;
  }

  ///<summary>
  /// Get book editions
  ///</summary>
  private static IResult GetAllBooks(ILibraryService service)
  {
    var bookEditions = service.GetAllBooks();
    return TypedResults.Json(bookEditions);
  }

  ///<summary>
  /// Get all book instances
  /// </summary>
  private static IResult GetAllBookInstances(ILibraryService service)
  {
    var bookInstances = service.GetAllBookInstances();
    return TypedResults.Json(bookInstances);
  }

  ///<summary>
  /// Get book instance using Id
  /// </summary>
  private static async Task<IResult> GetBookInstanceById(ILibraryService service, int id)
  {
    var bookInstance = await service.GetBookInstanceById(id);

    if (bookInstance == null)
      return TypedResults.NotFound($"Not found instance with id {id}");

    return TypedResults.Json(bookInstance);
  }

  ///<summary>
  /// Get book edition using ISBN
  /// </summary>
  private static async Task<IResult> GetBookByISBN(ILibraryService service, string isbn)
  {
    var bookEdition = await service.GetBookByISBN(isbn);

    if (bookEdition == null)
      return TypedResults.NotFound($"Not found edition with isbn -> {isbn}");

    return TypedResults.Json(bookEdition);
  }

  ///<summary>
  /// Create new book edition
  /// </summary>
  private static async Task<IResult> AddBookEdition(
    ILibraryService service,
    BookEditionDTO editionDTO
  )
  {
    var response = await service.AddBookEdition(editionDTO);

    if (!response.Result)
      return TypedResults.BadRequest(response.Message);

    return TypedResults.Created($"api/v1/library/items/by/{editionDTO.ISBN}");
  }

  ///<summary>
  /// Create new book instances
  /// </summary>
  /// <param name="service">library service</param>
  /// <param name="isbn">ISBN code of book edition, which instances need to add</param>
  /// <param name="amount">amount of books</param>
  private static async Task<IResult> AddBookInstances(
    ILibraryService service,
    string isbn,
    int amount
  )
  {
    var response = await service.AddBookInstances(isbn, amount);

    if (!response.Result)
      return TypedResults.BadRequest(response.Message);

    return TypedResults.Ok(response.Message);
  }

  ///<summary>
  /// Update book edition info
  /// </summary>
  private static async Task<IResult> UpdateBookEdition(
    ILibraryService service,
    string isbn,
    BookEditionDTO newBookEditionInfo
  )
  {
    var response = await service.UpdateBookEdition(isbn, newBookEditionInfo);

    if (!response.Result)
      return TypedResults.BadRequest(response.Message);

    return TypedResults.Ok(response.Message);
  }

  ///<summary>
  /// Update book instance data
  /// </summary>
  /// <remarks>
  /// The only data of book instance which is changing is date of taking and returning
  /// </remarks>
  private static async Task<IResult> UpdateBookInstance(
    ILibraryService service,
    int id,
    BookInstanceDTO newBookInstance
  )
  {
    var response = await service.UpdateBookInstance(id, newBookInstance);

    if (!response.Result)
      return TypedResults.BadRequest(response.Message);

    return TypedResults.Ok(response.Message);
  }

  ///<summary>
  /// Delete book edition
  /// </summary>
  private static async Task<IResult> DeleteBookEdition(ILibraryService service, string isbn)
  {
    var response = await service.DeleteBookEdition(isbn);

    if (!response.Result)
      return TypedResults.NotFound(response.Message);

    return TypedResults.NoContent();
  }

  ///<summary>
  /// Delete book instance
  /// </summary>
  private static async Task<IResult> DeleteBookInstance(ILibraryService service, int id)
  {
    var response = await service.DeleteBookInstance(id);

    if (!response.Result)
      return TypedResults.NotFound(response.Message);

    return TypedResults.NoContent();
  }

  ///<summary>
  /// Get authentication token
  ///</summary>
  public static IResult GetToken(ITokenService tokenService, string? username)
  {
    var token = tokenService.GetTokenByUsername(username);

    if (String.IsNullOrEmpty(token))
      return TypedResults.BadRequest("unable to create token, check input parameters");

    return TypedResults.Text(token);
  }
}
