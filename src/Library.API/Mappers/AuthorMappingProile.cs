using AutoMapper;
using Library.API.Models;
using Library.API.Models.DTO;

namespace Library.API.Mappers;

public class AuthorMappingProfile : Profile
{
  public AuthorMappingProfile()
  {
    CreateMap<Author, AuthorDTO>();
  }
}

