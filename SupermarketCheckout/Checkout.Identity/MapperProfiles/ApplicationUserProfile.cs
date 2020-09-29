using AutoMapper;
using Checkout.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Identity.MapperProfiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<RegistrationInfo, Models.ApplicationUserDto>()
                .ForMember(
                    dest => dest.SecurityStamp,
                    opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
