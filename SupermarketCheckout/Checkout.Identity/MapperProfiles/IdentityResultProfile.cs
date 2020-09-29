using AutoMapper;
using Checkout.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Identity.MapperProfiles
{
    public class IdentityResultProfile : Profile
    {
        public IdentityResultProfile()
        {
            CreateMap<IdentityResult, Models.AuthResponseDto>()
                .ForMember(
                    dest => dest.StatusEnum,
                    opt => opt.MapFrom(src => src.Succeeded ? AuthResult.Success : AuthResult.Error))
                .ForMember(
                    dest => dest.Message,
                    opt => opt.MapFrom(src => mapErrors(src.Errors)));
        }

        private string mapErrors(IEnumerable<IdentityError> errors)
        {
            if (errors?.Any() ?? false)
            {
                return null;
            }

            return string.Join(", ", errors.Select(e => e.Description));
        }
    }
}
