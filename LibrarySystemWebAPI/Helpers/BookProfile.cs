
using AutoMapper;
using LibrarySystemWebAPI.Dtos.BookDtos;
using LibrarySystemWebAPI.Entities;

namespace LibrarySystemWebAPI.Helpers
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, GetBookResponse>()
                .ForMember(dest => dest.BookId,
                    opt => opt.MapFrom(src => src.Id))

                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author.Name));
        }
    }
}
