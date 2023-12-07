using AutoMapper;
using Library.API.Models;
using Library.API.Models.DTO;

namespace Library.API.Mappers;

public class GenreMappingProfile : Profile
{
  public GenreMappingProfile()
  {
    CreateMap<Genre, GenreDTO>();
  }
}

