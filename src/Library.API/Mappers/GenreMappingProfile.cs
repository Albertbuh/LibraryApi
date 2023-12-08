using AutoMapper;

namespace Library.API.Mappers;

public class GenreMappingProfile : Profile
{
  public GenreMappingProfile()
  {
    CreateMap<Genre, GenreDTO>().ReverseMap();
  }
}

