using AutoMapper;
using Library.API.Models;
using Library.API.Models.DTO;

namespace Library.API.Mappers;

public class BookEditionMappingProfile : Profile
{
  public BookEditionMappingProfile()
  {
    CreateMap<BookEdition, BookEditionDTO>();
  }
}

