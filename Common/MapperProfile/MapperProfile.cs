using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Common.MapperProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CounselorAddDTO, Counselor>()
            .ForMember(dest => dest.image_url, opt => opt.Ignore());
               

        }
    }

}
