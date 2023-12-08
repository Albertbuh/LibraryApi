using AutoMapper;

namespace Library.API.Mappers;

public class BookEditionMappingProfile : Profile
{
  public BookEditionMappingProfile()
  {
    CreateMap<BookEdition, BookEditionDTO>().ReverseMap();
  }
}

