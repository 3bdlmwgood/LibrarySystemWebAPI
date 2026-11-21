
using AutoMapper;
using LibrarySystemWebAPI.Dtos.AuthorDtos;
using LibrarySystemWebAPI.Entities;

namespace LibrarySystemWebAPI.Helpers
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, GetAuthorResponse>()
                .ForMember(dest => dest.AuthorId,
                    opt => opt.MapFrom(src => src.Id))

                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Name));
        }
    }
}
