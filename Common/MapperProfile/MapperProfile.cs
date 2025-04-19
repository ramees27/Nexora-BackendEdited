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

            CreateMap<BookingRequestDto, Booking>()
            .ForMember(dest => dest.status, opt => opt.Ignore())
            .ForMember(dest => dest.created_at, opt => opt.Ignore())
            .ForMember(dest => dest.booking_id, opt => opt.Ignore()) 
            .ForMember(dest => dest.student_id, opt => opt.Ignore())
            .ForMember(dest => dest.counselor_id, opt => opt.Ignore());



            CreateMap<ReviewAddDTO, Review>()
           .ForMember(dest => dest.rating_id, opt => opt.Ignore()) // Automatically generate a new GUID
           .ForMember(dest => dest.student_id, opt => opt.Ignore()) // To be set manually in the service method
           .ForMember(dest => dest.counselor_id, opt => opt.Ignore()) // To be set manually in the service method
           .ForMember(dest => dest.booking_id, opt => opt.Ignore());
        }


    } 
}


