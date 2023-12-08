using AutoMapper;

namespace Library.API.Mappers;

public class AuthorMappingProfile : Profile
{
  public AuthorMappingProfile()
  {
    CreateMap<Author, AuthorDTO>().ReverseMap();
  }
}

