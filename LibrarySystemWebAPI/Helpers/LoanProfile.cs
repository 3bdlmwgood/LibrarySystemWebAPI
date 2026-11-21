
using AutoMapper;
using LibrarySystemWebAPI.Dtos.LoanDtos;
using LibrarySystemWebAPI.Entities;

namespace LibrarySystemWebAPI.Helpers
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<Loan, GetLoanResponse>()
                .ForMember(dest => dest.BookTitle,
                    opt => opt.MapFrom(src => src.Book.Title))

                .ForMember(dest => dest.BorrowerName,
                    opt => opt.MapFrom(src => src.Borrower.Name));
        }
    }
}
