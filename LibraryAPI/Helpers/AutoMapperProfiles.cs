using System;
using System.Linq;
using AutoMapper;
using LibraryAPI.Dtos;
using LibraryAPI.Models;

namespace LibraryAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Book, BorrowedBooksForListDto>()
                .ForMember(dto => dto.Readers,
                    opt => opt
                        .MapFrom(x => x.Loans.Select(y => y.Reader).ToList()));
            CreateMap<LoanForCreationDto, Loan>().ReverseMap();
            CreateMap<BookForRegisterDto, Book>();
            CreateMap<Book, BookForDetailDto>();
                //.ForMember(dest => dest.IsBorrowed, opt =>
                //    opt.MapFrom(src => src.Loans.Where(p => p.BookId == src.Id)));
            CreateMap<BookForUpdateDto, Book>();


        }
    }
}