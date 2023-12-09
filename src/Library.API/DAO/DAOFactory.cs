namespace Library.API.DAO;

public class DAOFactory
{
  private ILibraryDAO libraryDao = new LibraryDAO();

  public static DAOFactory Create()
  {
    return new DAOFactory();
  }

  public ILibraryDAO CreateLibraryDAO()
  {
    return libraryDao;
  }

}
