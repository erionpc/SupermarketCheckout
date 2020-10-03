using AutoMapper;
using Checkout.Identity.Models;
using Microsoft.AspNetCore.Identity;
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
            CreateMap<RegistrationInfo, Models.ApplicationUser>()
                .ForMember(
                    dest => dest.SecurityStamp,
                    opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<IdentityUser, Models.ApplicationUser>();
            CreateMap<Models.ApplicationUser, IdentityUser>();
        }
    }
}
