using AutoMapper;
using Library.API.Infrastructure;
using Library.API.Repositories;
using Library.API.Services.Exceptions;

// using Library.API.Repositories.Exceptions;

namespace Library.API.Services;

public class LibraryService : ILibraryService
{
  private ILibraryRepository repository;
  private ILogger logger;
  private IMapper mapper;

  public LibraryService(IMapper mapper, LibraryContext context)
  {
    repository = RepositoryFactory.Create().CreateLibraryRepository(context);
    this.mapper = mapper;

    using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
    logger = factory.CreateLogger("LibraryService");
  }

  public IList<BookEditionDTO> GetAllBooks()
  {
    IList<BookEditionDTO>? result = null;
    try
    {
      var bookEditions = repository.GetAllBooks();
      result = mapper.Map<List<BookEditionDTO>>(bookEditions);
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }
    return result;
  }

  public IList<BookInstanceDTO> GetAllBookInstances()
  {
    IList<BookInstanceDTO>? result = null;
    try
    {
      var bookInstances = repository.GetAllBookInstances();
      result = mapper.Map<List<BookInstanceDTO>>(bookInstances);
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }
    return result;
  }

  public async Task<BookInstanceDTO?> GetBookInstanceById(int id)
  {
    if (id <= 0)
      return null;

    BookInstanceDTO? result;

    try
    {
      var bookInstance = await repository.GetBookInstanceById(id);
      result = mapper.Map<BookInstanceDTO>(bookInstance);
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }

    return result;
  }

  public async Task<BookEditionDTO?> GetBookByISBN(string isbn)
  {
    if (!IsISBNValid(isbn))
      return null;

    BookEditionDTO? result;

    try
    {
      var bookEdition = await repository.GetBookByISBN(isbn);
      result = mapper.Map<BookEditionDTO>(bookEdition);
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException($"Error in getting book by ISBN -> {isbn}", e);
    }

    return result;
  }

  public async Task<LibraryServiceResponse> AddBookEdition(BookEditionDTO bookEditionDTO)
  {
    if (!IsISBNValid(bookEditionDTO.ISBN))
      return new LibraryServiceResponse("Invalid ISBN", false);

    var response = new LibraryServiceResponse(
      $"BookEdition with ISBN {bookEditionDTO.ISBN} added to database",
      true
    );

    try
    {
      var isEditionAlreadyCreated = await GetBookByISBN(bookEditionDTO.ISBN) != null;
      if (!isEditionAlreadyCreated)
      {
        var edition = mapper.Map<BookEdition>(bookEditionDTO);
        var isAdded = await repository.AddBookEdition(edition);

        if (!isAdded)
        {
          response = new LibraryServiceResponse(
            $"Unable to add book edition isbn -> {edition.ISBN}",
            false
          );
        }
      }
      else
        response = new LibraryServiceResponse(
            $"Book with such ISBN -> {bookEditionDTO.ISBN} already in database",
            false
            );
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException($"Error while adding book with {bookEditionDTO.ISBN}", e);
    }
    return response;
  }

  public async Task<LibraryServiceResponse> AddBookInstances(string isbn, int amount)
  {
    if (!IsISBNValid(isbn))
      return new LibraryServiceResponse("Invalid ISBN", false);
    if (amount < 0 || amount > 100)
      return new LibraryServiceResponse("Incorrect amount. Amount is [1, 100]");

    var result = new LibraryServiceResponse(
      $"Added new book instances for {isbn} in {amount} copies",
      true
    );

    var edition = await repository.GetBookByISBN(isbn);
    bool isAdded = false;
    if (edition != null)
    {
      var instances = new BookInstance[amount];
      for (int i = 0; i < amount; i++)
        instances[i] = new BookInstance(edition);

      isAdded = await repository.AddBookInstances(instances);

      if (!isAdded)
      {
        result = new LibraryServiceResponse(
          $"Unable to add instances for isbn -> {isbn}, check if it exists",
          false
        );
      }
    }

    return result;
  }

  public async Task<LibraryServiceResponse> DeleteBookEdition(string isbn)
  {
    if (!IsISBNValid(isbn))
      return new LibraryServiceResponse("Invalid ISBN", false);

    try
    {
      bool isDeleted = await repository.DeleteBookEdition(isbn);

      if (!isDeleted)
        return new LibraryServiceResponse($"Not found edition with isbn -> {isbn}", false);
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }

    return new LibraryServiceResponse($"Successfull deletion of book edition {isbn}", true);
  }

  public async Task<LibraryServiceResponse> DeleteBookInstance(int id)
  {
    if (id <= 0)
      return new LibraryServiceResponse("Id < 0", false);

    try
    {
      bool isDeleted = await repository.DeleteBookInstance(id);

      if (!isDeleted)
        return new LibraryServiceResponse($"Instance with id -> {id} not founded", false);
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }
    return new LibraryServiceResponse($"Successfull deletion of book instance with id {id}", true);
  }

  public async Task<LibraryServiceResponse> UpdateBookEdition(
    string isbn,
    BookEditionDTO newInfoDTO
  )
  {
    if (!IsISBNValid(isbn))
      return new LibraryServiceResponse($"Invalid ISBN {isbn}", false);
    if (!IsISBNValid(newInfoDTO.ISBN))
      return new LibraryServiceResponse(
        $"Invalid ISBN of BookEdition update info {newInfoDTO.ISBN}",
        false
      );

    var response = new LibraryServiceResponse($"Book Edition has been updated", true);

    try
    {
      var newInfo = mapper.Map<BookEdition>(newInfoDTO);
      bool isUpdated = await repository.UpdateBookEdition(isbn, newInfo);

      if (!isUpdated)
        response = new LibraryServiceResponse(
          $"Book edition {isbn} not found, update impossible",
          false
        );
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }
    return response;
  }

  public async Task<LibraryServiceResponse> UpdateBookInstance(int id, BookInstanceDTO newInfoDTO)
  {
    if (id <= 0)
      return new LibraryServiceResponse("Id < 0", false);
    if (!IsISBNValid(newInfoDTO.ISBN))
      return new LibraryServiceResponse(
        $"Invalid ISBN of Book instance update info: {newInfoDTO.ISBN}",
        false
      );

    LibraryServiceResponse response = new LibraryServiceResponse(
      $"New inforamtion for book instance with id -> {id} successfully loaded",
      true
    );

    try
    {
      var newInfo = mapper.Map<BookInstance>(newInfoDTO);
      var isUpdated = await repository.UpdateBookInstance(id, newInfo);

      if (!isUpdated)
        response = new LibraryServiceResponse(
          $"Unable to update book with id -> {id}, try to check if this book exists",
          false
        );
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }

    return response;
  }

  private bool IsISBNValid(string isbn)
  {
    const string pattern = @"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$";
    return System.Text.RegularExpressions.Regex.IsMatch(isbn, pattern);
  }
}
