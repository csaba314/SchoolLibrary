﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MVC.ViewModels;
using Services.Models;

namespace MVC.App_Start
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            Mapper.CreateMap<BookDTO, IBook>().ForMember(m => m.ID, opt => opt.Ignore());
            Mapper.CreateMap<IBook, BookDTO>();

            Mapper.CreateMap<AuthorDTO, IAuthor>().ForMember(m => m.ID, opt => opt.Ignore());
            Mapper.CreateMap<IAuthor, AuthorDTO>();

            Mapper.CreateMap<CustomerDTO, ICustomer>().ForMember(m => m.Id, opt => opt.Ignore());
            Mapper.CreateMap<ICustomer, CustomerDTO>();

            Mapper.CreateMap<RentalDTO, IRental>();
            Mapper.CreateMap<IRental, RentalDTO>();
        }
    }
}