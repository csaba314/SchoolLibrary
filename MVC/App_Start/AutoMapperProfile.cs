using System;
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
        }
    }
}