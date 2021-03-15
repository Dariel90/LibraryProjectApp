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
            CreateMap<BookForUpdateDto, Book>();
            CreateMap<Loan, LoanToReturnDto>()
                .ForMember(dto => dto.BookName,opt => opt
                    .MapFrom(x => x.Book.Name))
                .ForMember(dto => dto.ReaderName, opt => opt
                    .MapFrom(x => x.Reader.Name));
            CreateMap<Reader, ReaderForDetailDto>().ReverseMap();
            CreateMap<ReaderForDetailDto, Reader>();


        }
    }
}