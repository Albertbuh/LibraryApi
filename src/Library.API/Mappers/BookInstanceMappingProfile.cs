using AutoMapper;
using Library.API.Models;
using Library.API.Models.DTO;

namespace Library.API.Mappers;

public class BookInstanceMappingProfile : Profile
{
  public BookInstanceMappingProfile()
  {
    CreateMap<BookInstance, BookInstanceDTO>()
      .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.Book.ISBN))
      .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Book.Title))
      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Book.Description))
      .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.Book.Authors))
      .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Book.Genres));
  }
}
