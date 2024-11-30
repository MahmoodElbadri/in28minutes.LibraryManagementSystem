﻿using AutoMapper;
using Library.DTOs;
using Library.Models;

namespace Library.Mappings;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<Book, BookUpdateRequest>().ReverseMap();
        CreateMap<Book, BookAddRequest>().ReverseMap();
        CreateMap<Book, BookResponse>().ReverseMap();

        CreateMap<Category, CategoryUpdateRequest>().ReverseMap();
        CreateMap<Category, CategoryAddRequest>().ReverseMap();
        CreateMap<Category, CategoryResponse>().ReverseMap();
    }
}
